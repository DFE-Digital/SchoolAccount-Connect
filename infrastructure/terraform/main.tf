data "azurerm_app_configuration" "app_config" {
  name                = "s268d01appcs-sa-shared"
  resource_group_name = "s268d01rg-uks-sa-shared"
}


data "azurerm_key_vault" "kv" {
  name                = "s268d01kvs-sa-shared"
  resource_group_name = "s268d01rg-uks-sa-shared"
}

data "azurerm_key_vault_secret" "ai_key" {
  name         = "AppInsightsInstrumentationKey"
  key_vault_id = data.azurerm_key_vault.kv.id
}


resource "azurerm_container_app" "app" {
  name                         = "schoolaccount-connect-app"
  resource_group_name          = "s268d01rg-uks-sa-poc"
  container_app_environment_id = data.azurerm_container_app_environment.env.id
  revision_mode                = "Single"

  identity {
    type = "SystemAssigned"
  }

  secret {
    name = "storageconnectionstring"
    key_vault_secret_id = "${data.azurerm_key_vault.kv.vault_uri}secrets/StorageConnectionString"
    identity = "System"
  }

  secret {
    name = "sasqlconnectionstring"
    key_vault_secret_id = "${data.azurerm_key_vault.kv.vault_uri}secrets/saSQLConnectionString"
    identity = "System"
  }

  secret {
    name = "dfesigninclientsecret"
    key_vault_secret_id = "${data.azurerm_key_vault.kv.vault_uri}secrets/DFESignInClientSecret"
    identity = "System"
  }
  
  template {
    container {
      name   = "schoolaccount-connect-app"
      image  = "ghcr.io/dfe-digital/schoolaccount-connect:${var.image_tag}"
      cpu    = 0.25
      memory = "0.5Gi"
      
      #Env Var (Terraform)
      env {
        name  = "ASPNETCORE_URLS"
        value = "http://0.0.0.0:8080"
      }
      
      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = var.aspnetcore_enviroment
      }

      env {
        name  = "AzureAppConfiguration__Enabled"
        value = "true"
      }

      env {
        name  = "AzureAppConfiguration__Endpoint"
        value = data.azurerm_app_configuration.app_config.endpoint
      }

      env {
        name  = "AzureAppConfiguration__IsEmulated"
        value = "false"
      }

      env {
        name  = "DfeSignIn__ClientId"
        value = "SCHOOLACCOUNT"
      }

      env {
        name  = "DfeSignIn__CallbackUrl"
        value = "/"
      }

      env {
        name  = "DfeSignIn__SignoutCallbackUrl"
        value = "/account"
      }

      env {
        name  = "DfeSignIn__SignoutRedirectUrl"
        value = "/account/SignedOut"
      }

      env {
        name  = "DfeSignIn__LoginPath"
        value = "/start"
      }

      env {
        name  = "DfeSignIn__CookieName"
        value = "sa-login"
      }

      env {
        name  = "DfeSignIn__CookieExpireTimeSpanInMinutes"
        value = "60"
      }

      env {
        name  = "DfeSignIn__GetClaimsFromUserInfoEndpoint"
        value = "true"
      }

      env {
        name  = "DfeSignIn__SaveTokens"
        value = "true"
      }

      env {
        name  = "DfeSignIn__SlidingExpiration"
        value = "true"
      }

      env {
        name  = "DfeSignIn__Scopes__0"
        value = "openid"
      }

      env {
        name  = "DfeSignIn__Scopes__1"
        value = "email"
      }

      env {
        name  = "DfeSignIn__Scopes__2"
        value = "profile"
      }

      env {
        name  = "DfeSignIn__Scopes__3"
        value = "organisation"
      }

      env {
        name  = "DfeSignIn__AccessDeniedPath"
        value = "/error/401"
      }

      env {
        name  = "DfeSignIn__DiscoverRolesWithPublicApi"
        value = "false"
      }

      #Config App Configuration
      env {
        name  = "DfeSignIn__PublicURL"
        value = "https://placeholder.local"
      }

      env {
        name  = "DfeSignIn__Scope"
        value = "https://placeholder.local"
      }

      env {
        name  = "DfeSignIn__MetaDataUrl"
        value = "https://placeholder.local"
      }
      
      #Secrets Key Vault
      env {
        name        = "StorageConnectionString"
        secret_name = "storageconnectionstring"
      }

      env {
        name        = "ConnectionStrings__SchoolAccount"
        secret_name = "sasqlconnectionstring"
      }

      env {
        name        = "DfeSignIn__ClientSecret"
        secret_name = "dfesigninclientsecret"
      }

      env {
        # temporary build in Terraform from App Insight key from KV secrets
        name  = "APPLICATIONINSIGHTS_CONNECTION_STRING"
        value = "InstrumentationKey=${data.azurerm_key_vault_secret.ai_key.value};IngestionEndpoint=https://uksouth-0.in.applicationinsights.azure.com/"
      }
      
    }
  }

  ingress {
    external_enabled = true
    target_port      = 8080
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }
}

data "azurerm_container_app_environment" "env" {
  name                = "s268d01ace-sa-poc"
  resource_group_name = "s268d01rg-uks-sa-poc"
}

resource "azurerm_role_assignment" "app_config_reader" {
  scope                = data.azurerm_app_configuration.app_config.id
  role_definition_name = "App Configuration Data Reader"
  principal_id         = azurerm_container_app.app.identity[0].principal_id
  depends_on = [
    azurerm_container_app.app
  ]
  skip_service_principal_aad_check = true
}

resource "azurerm_key_vault_access_policy" "aca_access" {
  key_vault_id = data.azurerm_key_vault.kv.id

  tenant_id = azurerm_container_app.app.identity[0].tenant_id
  object_id = azurerm_container_app.app.identity[0].principal_id

  secret_permissions = [
    "Get",
    "List"
  ]

  depends_on = [
    azurerm_container_app.app
  ]
}
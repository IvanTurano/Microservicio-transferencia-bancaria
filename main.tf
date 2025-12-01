terraform {
    required_version = ">= 1.2"
    required_providers {
        azurerm = {
            source  = "hashicorp/azurerm"
            version = "~> 4.2"
        }
    }
}

provider "azurerm" {
    features {
        resource_group {
            prevent_deletion_if_contains_resources = false
        }
    }
}

resource "azurerm_resource_group" "rg" {
    name = "rg-transfer-chilecentral"
    location = "chilecentral"
}

# Shared App Service Plan for all microservices
resource "azurerm_service_plan" "plan_shared" {
    name = "plan-shared-chilecentral"
    location = "chilecentral"
    resource_group_name = azurerm_resource_group.rg.name
    os_type = "Linux"
    sku_name = "F1"
}

#MS API Gateway
resource "azurerm_linux_web_app" "appservice-apigateway"{
  name = "appserv-apigateway-chilecentral-1"
  location = "chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id = azurerm_service_plan.plan_shared.id
  site_config {
    always_on = false
    application_stack {
      docker_image_name = "nginx:latest"
    }
  }
}

#MS Transaction
resource "azurerm_linux_web_app" "appservice-transaction"{
  name = "appserv-transaction-chilecentral-1"
  location = "chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id = azurerm_service_plan.plan_shared.id
  site_config {
    always_on = false
    application_stack {
      docker_image_name = "nginx:latest"
    }
  }
}

#MS Balance
resource "azurerm_linux_web_app" "appservice-balance"{
  name = "appserv-balance-chilecentral-1"
  location = "chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id = azurerm_service_plan.plan_shared.id
  site_config {
    always_on = false
    application_stack {
      docker_image_name = "nginx:latest"
    }
  }
}

#MS Transfer
resource "azurerm_linux_web_app" "appservice-transfer"{
  name = "appserv-transfer-chilecentral-1"
  location = "chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id = azurerm_service_plan.plan_shared.id
  site_config {
    always_on = false
    application_stack {
      docker_image_name = "nginx:latest"
    }
  }
}

#MS Notification
resource "azurerm_linux_web_app" "appservice-notification"{
  name = "appserv-notification-chilecentral-1"
  location = "chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id = azurerm_service_plan.plan_shared.id
  site_config {
    always_on = false
    application_stack {
      docker_image_name = "nginx:latest"
    }
  }
}



#DB Transaction
resource "azurerm_mssql_server" "sql_server_transaction" {
  name = "sqlserver-transaction-chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  location = "chilecentral"
  version = "12.0"
  administrator_login = "adminuser"
  administrator_login_password = "Pass.12345@"
}

resource "azurerm_mssql_database" "sql_database_transaction" {
  name = "sqldb-transaction-chilecentral"
  server_id = azurerm_mssql_server.sql_server_transaction.id
  sku_name = "Basic"
  storage_account_type = "Local"
}


#DB Balance
resource "azurerm_mssql_server" "sql_server_balance" {
  name = "sqlserver-balance-chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  location = "chilecentral"
  version = "12.0"
  administrator_login = "adminuser"
  administrator_login_password = "Pass.12345@"
}

resource "azurerm_mssql_database" "sql_database_balance" {
  name = "sqldb-balance-chilecentral"
  server_id = azurerm_mssql_server.sql_server_balance.id
  sku_name = "Basic"
  storage_account_type = "Local"
}


#DB Transfer
resource "azurerm_mssql_server" "sql_server_transfer" {
  name = "sqlserver-transfer-chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  location = "chilecentral"
  version = "12.0"
  administrator_login = "adminuser"
  administrator_login_password = "Pass.12345@"
}

resource "azurerm_mssql_database" "sql_database_transfer" {
  name = "sqldb-transfer-chilecentral"
  server_id = azurerm_mssql_server.sql_server_transfer.id
  sku_name = "Basic"
  storage_account_type = "Local"
}



#DB Cosmos DB
resource "azurerm_cosmosdb_account" "cosmosdb_account_notification" {
  name = "account-notification-chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  location = "chilecentral"
  offer_type = "Standard"
  consistency_policy {
    consistency_level = "Session"
  }
  geo_location {
    location = "chilecentral"
    failover_priority = 0
  }
}

resource "azurerm_cosmosdb_sql_database" "cosmosdb_sql_notification" {
  name = "cosmosdb-notification"
  resource_group_name = azurerm_resource_group.rg.name
  account_name = azurerm_cosmosdb_account.cosmosdb_account_notification.name
}


# Azure Functions - No se puede crear en Azure para subcripciones de estudiantes
# #azure storage account
# resource "azurerm_storage_account" "storage_account_function" {
#   name = "stacfunceastus2"
#   resource_group_name = azurerm_resource_group.rg.name
#   location = "eastus2"
#   account_tier = "Standard"
#   account_replication_type = "LRS"
# }


# #Azure function
# resource "azurerm_service_plan" "plan_deadletter" {
#     name = "plan-deadletter-eastus2"
#     location = "eastus2"
#     resource_group_name = azurerm_resource_group.rg.name
#     os_type = "Linux"
#     sku_name = "Y1"
# }

# resource "azurerm_linux_function_app" "function_app" {
#   name = "funct-deadletter-eastus2"
#   location = "eastus2"
#   resource_group_name = azurerm_resource_group.rg.name
#   service_plan_id = azurerm_service_plan.plan_deadletter.id
#   storage_account_name = azurerm_storage_account.storage_account_function.name
#   storage_account_access_key = azurerm_storage_account.storage_account_function.primary_access_key
#   site_config {
#     always_on = false
#   }
# }


#Azure service bus
resource "azurerm_servicebus_namespace" "sb_transfer" {
  name = "servbus-transfer-chilecentral"
  location = "chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  sku = "Standard"

}

#App Insights
resource "azurerm_application_insights" "app_insights_transfer" {
  name = "appins-transfer-chilecentral"
  location = "chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  application_type = "web"
}


#Azure key vault
resource "azurerm_key_vault" "key_vault_transfer" {
  name = "keyvault-transfer-ch"
  location = "chilecentral"
  resource_group_name = azurerm_resource_group.rg.name
  sku_name = "standard"
  tenant_id = data.azurerm_client_config.current.tenant_id
}

data "azurerm_client_config" "current" {}
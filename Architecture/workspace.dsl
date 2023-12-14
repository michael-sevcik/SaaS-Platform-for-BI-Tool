workspace "MericaBI" "This workspace documents the architecture of the MericaBI system which enables managing of mapping customer database models to a given generic model, tranforming the data and menaging instances of Metabse" {
    !identifiers hierarchical

    model {
        custumer = person "Customer" "Manager of a manufacturing company"
        administrator = person "BI Administrator" "Person responsible for managing the BI system - creating new projects, managing users, etc."

        // Mapper
        mapper = softwareSystem "Mapper" "Enables mapping of customer database models to a given generic model" {
            mapperApp = container "Mapper single page application" "Provides tools for mapping data models" "TypeScript" {
            }

            mapperApi = container "Mapper API" "Provides API for mapping data models" "C#" {
                mapperController = component "Mapper Controller" "Handles mapping requests" {
                    tags "Controller"
                }

                mapperService = component "Mapper Service" "Handles mapping logic" {
                    tags "Service"
                }
            }

            mapperDatabase = container "Mapping Database" "Provides data persistence for mapping data" "TODO" {
                tags "Database"

            }

            # api_gateway = container "API Gateway" "Provides Rest API endpoints" "" "BackEnd" {
            #     // handles requests from UI
            #     request_controller = component "Request Handler" "Handles API requests"
            #     // routes requests from UI to Rest API
            #     routing_controller = component "Routing Controller" "Handles API endpoint routing"
            #     // handles real-time messaging using WebSocket
            #     web_socket_controller = component "WebSocket" "Provides WebSocket for real-time message handling"

            #     request_controller -> routing_controller "Transmits API requests"

            # }

            # rest_api = container "Rest API" "Handles Rest API endpoints" {
            #     // provides interface to query the database
            #     database_query_interface = component "Database Query Interface" "Handles database queries"
            #     // validates the integrity of data provided to database
            #     validator = component "Data Validator" "Provides data validation and integrity"

            #     // handles project API requests
            #     project_controller = component "Project Controller" "Handles project-related requests" {
            #         tags "Controller"
            #     }
            #     // handles user API requests
            #     user_controller = component "User Controller" "Handles user-related requests" {
            #         tags "Controller"
            #     }
            #     // handles mapping API requests
            #     mapping_controller = component "Mapping Controller" "Handles mapping-related requests" {
            #         tags "Controller"
            #     }
            #     // handles instance API requests
            #     instance_controller = component "Instance Controller" "Handles instance-related requests" {
            #         tags "Controller"
            #     }
            # } 

            notification_service = container "Notification Service" "Provides notification management" {
                tags "Service"

                // handles manual notification requests from UI (administrator wants to manually send a message)
                notification_request_handler = component "Request Handler" "Handles notification requests"
                // listens to database changes for notification sending
                database_listener = component "Database Listener" "Listens to database data changes"
                // handles notifications (processing, model)
                notification_handler = component "Notification Handler" "Handles notifications"
                // emits the notification to
            }
        }

        // BI management system
        bi_management = softwareSystem "BI Management System" "Enables management of BI projects (data mapping, spinning up new instances of Metabase)" {

        }
        

        live = deploymentEnvironment "Live" {
            deploymentNode "Client Computer" {
                deploymentNode "Web Browser" {
                    spa = infrastructureNode "Single Page Application" {
                        description "Provides exam system functionality via web browser"
                    }
                }
            }

            deploymentNode "Whole architecture" {
                
                deploymentNode "Backend Server" {
                    lbNode = infrastructureNode "Load Balancer" {
                        description "Balances incoming calls"
                    }

                    apiNode = infrastructureNode "Rest API Infrastructure" {
                        description "Provides Rest API endpoints"
                    }

                    deploymentNode "Database Enviroment" {
                        databaseNode = infrastructureNode "Database" {
                            tags "Database"
                            description "Provides data persistence"
                        }

                        messageDatabaseNode = infrastructureNode "Message Database" {
                            tags "Database"
                            description "Provides message history persistence"
                        }
                    }
                }

                deploymentNode "Service Infrastructure" {
                    notificationNode = infrastructureNode "Notification Service" {
                        description "Provides notification handler"
                    }

                    messageNode = infrastructureNode "Message Service" {
                        description "Provides message handler"
                    }
                }

                deploymentNode "Auth Server" {
                    authNode = infrastructureNode "Auth Service" {
                        description "Provides authentication and authorization functionality"
                    }
                }	
            }

            # apiNode -> authNode "Sends auth requests"
            # authNode -> apiNode "Sends back auth data"

            # spa -> lbNode "Calls API requests"
            # apiNode -> spa "Sends back requested data"
            # lbNode -> apiNode "Forwards API requests"

            # apiNode -> databaseNode "Sends data to persist"
            # apiNode -> messageDatabaseNode "Sends message history"

            # databaseNode -> apiNode "Sends back requested data"
            # messageDatabaseNode -> apiNode "Sends back message data"

            # apiNode -> notificationNode "Request notification"
            # notificationNode -> databaseNode "Subscribe to data changes"

            # messageNode -> messageDatabaseNode "Store message data"
            # messageDatabaseNode -> messageNode "Send back message history"
        }
    }

    views {
        # TODO: ADD system landscape view

        systemContext mapper "examSystemSystemContextDiagram" {
            include *
            autoLayout lr
        }

        container mapper "examSystemContainerDiagram" {
            autoLayout
            include *
        }

        deployment mapper "Live" "AmazonWebServicesDeployment" {
            autoLayout
            include *
        }

        styles {
            element "Person" {
                color #08427b
                fontSize 22
                shape Person
            }

            element "Software System" {
                background #1168bd
                color #ffffff
            }
            element "Container" {
                background #438dd5
                color #ffffff
            }
            
            element "Component" {
                background #c4c4c4
                color #000000
            }

            element "Service" {
                background #d4655d
            }
            element "Database" {
                shape Cylinder
                background #30469c
                color #ffffff
            }
            element "Controller" {
                background  #57b586
            }
            element "External" {
                background  #636363
            }
        }
    }
}

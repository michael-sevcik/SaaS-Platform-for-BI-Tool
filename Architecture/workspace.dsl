workspace "MericaBI" "This workspace documents the architecture of the MericaBI system which enables managing of mapping customer database models to a given generic model, tranforming the data and menaging instances of Metabse" {
    !identifiers hierarchical

    model {
        costumer = person "Customer" "Manager of a manufacturing company"
        administrator = person "BI Administrator" "Person responsible for managing the BI system - creating new projects, managing users, etc."

        k8sCluster  = softwareSystem "Metabase Deployment System" "Provides infrastructure for deploying containers" {
            tags "External"

            metabaseInstance = container "Metabase" "Enables costumer to explore their data" "" {
                tags "External"
            }

            mssql = container "MS SQL" "Provides data persistence for Metabase" "MS SQL" {
                tags "Database"
            }

            k8sAPI = container "Kubernetes API" "Provides API for managing Kubernetes cluster" "kube-apiserver" {
                tags "Service"
            }

            // relations
            k8sAPI -> metabaseInstance "Manages Metabase instances"
            k8sAPI -> mssql "Manages databases"
            metabaseInstance -> mssql "Uses"
            costumer -> metabaseInstance "Uses Metabase to explore data"
        }

        biManagementSystem = softwareSystem "BI Management Platform" "Enables management of BI projects (data mapping, spinning up new instances of Metabase)" {
            managementApp = container "BI Management App" "Enables management of BI projects (data mapping, spinning up new instances of Metabase)" "ASP.NET Core Blazor`" {
                group management {            
                managementApp = component "BI Management App" "Provides tools for managing BI projects" "ASP.NET Core Blazor`" {
                }

                managementApi = component "BI Management API" "Provides API for managing BI projects" "ASP.NET Core Blazor" {
                    # projectManager = component "Project Manager" "Manages projects" {
                    #     tags "Controller"
                    # }
                }

                // relations
                managementApp -> ManagementApi "Sends management requests"
                managementApi -> ManagementApp "Provides project informtion"
            } 

            group mapping {
                mapperApp = component "Mapper App" "Single page application that provides tools for mapping data models" "TypeScript" {
                }

                mapperApi = component "Mapper API" "Provides API for mapping data models" "C#" {
                    # mappingManager = component "Mapper Manager" "Handles mapping requests" {
                    #     tags "Controller"
                    # }

                    # mapperService = component "Mapper Service" "Handles mapping logic" {
                    #     tags "Service"
                    # }
                }

                // relations
                mapperApp -> mapperApi "Sends mapping data"

                 // TODO: Consider diffrent description
                mapperApi -> mapperApp "Provides scaffolded data"
            }

            authentificationService = component "AuthentificationService" "C#" {
                tags "Service"
            
                # // TODO: Consider splitting user management and authentification logic
                # identityManager = component "Identity Manager" "Menages users and authentications" "ASP.NET Identity" {
                #     tags "Controller"

                # }
            }

            // TODO: Rename
            metabaseDeploymentService = component "Metabase Deployment Service" "C#" {
                tags "Service"
                
                # deploymentManager = component "Deployment Manager" "Manages Metabase deployments" {
                #     tags "Controller"
                # }

            }

            metabaseDeploymentService -> k8sCluster.k8sAPI "Deploys data and Metabase instances"

            // TODO: MOVE to container
            projectDatabase = component "BI Management Database" "Provides data persistence for BI projects - data mappings, user infromation, project statuses" "MS SQL" {
                tags "Database"
            }

            # // relations // TODO: add authentification service relations
            # mapperApi.mappingManager -> managementApi.projectManager "Updates project status"
            # mapperApi.mappingManager -> projectDatabase "Manages mapping data"
            # managementApi.projectManager -> projectDatabase "Manages project data"
            }

            

        }
        
        // relations 
        costumer -> biManagementSystem.managementApp.mapperApp "Maps his data"
        administrator -> biManagementSystem.managementApp.managementApp "Manages BI projects - creates new, starts deployment of Metabase instances, etc."

        // Mapper
        mapper = softwareSystem "Mapper" "Enables mapping of customer database models to a given generic model" {
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
        systemLandscape biManagementSystem "systemLandscape" {
            include *
            autoLayout
        }
        systemContext biManagementSystem "biManagementSystemSystemContextDiagram" {
            include *
            autoLayout lr
        }

        container k8sCluster "k8sClusterSystemContextDiagram" {
            include *
            autoLayout lr
        }

        container biManagementSystem "biManagementSystemContainerDiagram" {
            autoLayout
            include *
        }

        component biManagementSystem.managementApp "biManagementSystemManagementApiComponentDiagram" {
            autoLayout
            include *
        }
        
        # component biManagementSystem.mapperApi "biManagementSystemmapperApiComponentDiagram" {
        #     autoLayout
        #     include *
        # }

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




workspace "MericaBI" "This workspace documents the architecture of the MericaBI system which enables managing of mapping customer database models to a given generic model, tranforming the data and menaging instances of Metabse" {
    !identifiers hierarchical

    model {
        costumer = person "Customer" "Manager of a manufacturing company"
        administrator = person "BI Administrator" "Person responsible for managing the BI system "
        # administrator = person "BI Administrator" "Person responsible for managing the BI system - creating new projects, managing users, etc."

        
        email = softwaresystem "E-mail System" "" "Existing System" {
            tags "External"
            email -> costumer "Sends notifications to costumers"
            email -> administrator "Sends notifications to administrators" 
        }

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
                
                mediator = component "Mediator" "Provides communication between modules" "C#: MediatR"

                userManagementModule = component "User Management Module" "Provides user management functionality" "C#"
                dataIntegrationModule = component "Data Integration Module" "Provides data integration functionality" "C# + TypeScript" 
                mapperLibrary = component "Mapper Library" "Provides mapping functionality" "TypeScript + JoinJS"
                metabaseDeploymentModule = component "Metabase Deployment Module" "Menages Kubernetes deployment" "C#: "


                metabaseDeploymentModule -> k8sCluster.k8sAPI "Deploys database and Metabase instances"

                dataIntegrationModule -> mapperLibrary "Integrates mapping library"
                userManagementModule -> mediator "Subscribes and publishes user events"
                dataIntegrationModule -> mediator "Subscribes and publishes data integration events"
                metabaseDeploymentModule -> mediator "Subscribes and publishes deployment events"
                
                userManagementModule -> email "Sends e-mails to users" "SMTP"
            }

            
            projectDatabase = container "BI Management Database" "Provides data persistence for BI projects - data mappings, user infromation, project statuses" "MS SQL" {
                tags "Database"
            }

            // relations
            managementApp -> projectDatabase "Uses"
            managementApp.dataIntegrationModule -> projectDatabase "Saves data mappings"
            managementApp.userManagementModule -> projectDatabase "Saves user information"
            managementApp.metabaseDeploymentModule -> projectDatabase "Saves project statuses"


        }
        

        // relations 
        costumer -> biManagementSystem.managementApp.dataIntegrationModule "Maps his data"
        administrator -> biManagementSystem.managementApp.userManagementModule "Manages BI projects - creates new, starts deployment of Metabase instances, etc."


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
        systemContext biManagementSystem "biManagementSystemSystemContextDiagram" {
            include *
        }

        container k8sCluster "k8sClusterSystemContextDiagram" {
            include *
        }

        container biManagementSystem "biManagementSystemContainerDiagram" {
            autoLayout
            include *
        }

        component biManagementSystem.managementApp "biManagementSystemManagementApiComponentDiagram" {
            autoLayout
            include *
        }
        

        styles {
            relationship "Relationship"{
                fontSize 30
            }   

            element "Element" {
                fontSize 30

            }
            element "Person" {
                color #08427b
                fontSize 24
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
                background #85bbf0
                color #000000

            }

            element "Failover" {
                opacity 25
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
                background  #737373
            }
        }
    }
}




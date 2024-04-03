workspace "SaaSBI" "This workspace documents the architecture of the SaaSBI system which enables managing of mapping customer database models to a given generic model, tranforming the data and managing instances of Metabse" {
    !identifiers hierarchical

    model {
        costumer = person "Customer" "Employee of a manufacturing company"
        administrator = person "BI Administrator" "Person responsible for managing the BI system "

        
        email = softwaresystem "E-mail System" "" "Existing System" {
            tags "External"
            email -> costumer "Sends notifications to costumers"
            email -> administrator "Sends notifications to administrators" 
        }

        k8sCluster  = softwareSystem "Metabase Deployment System" "Provides infrastructure for deploying containers using Kubernetes" "k8s cluster" {
            tags "External"

            metabaseInstance = container "Metabase" "Enables costumer to explore their data" "" {
                tags "External" 
            }

            mssql = container "MS SQL" "Provides data persistence for Metabase" "MS SQL" {
                tags "Database"
            }

            k8sAPI = container "Kubernetes API" "Provides API for managing Kubernetes cluster" "kube-apiserver" {
                tags "Service" "Kubernetes - api"
            }

            // relations
            k8sAPI -> metabaseInstance "Manages Metabase instances"
            k8sAPI -> mssql "Manages databases"
            metabaseInstance -> mssql "Uses"
            costumer -> metabaseInstance "Uses Metabase to explore data"
        }

        biManagementSystem = softwareSystem "BI Management Platform" "Enables management of BI projects (data mapping, deploying new instances of Metabase)" {
            managementHtml = container "Management HTML" "Provides management functionality in web browser" "HTML + CSS + JavaScript" 


            managementApp = container "BI Management App" "Enables management of BI projects (user management, data mapping, deploying Metabase)" "ASP.NET Core Blazor`" {

                blazorServer = component "Blazor Server" "Renders the management UI and executes user requests" "C#"
                managementHtml -> blazorServer "Sends requests to" "SignalR" {
                    tags "excludeInDeployment"
                }
                blazorServer -> managementHtml "Delivers content to" "HTTPS + SignalR" {
                    tags "excludeInDeployment"
                }
                
                # mediator = component "Mediator" "Provides communication between modules" "C#: MediatR"

                userManagementModule = component "User Management Module" "Provides user management functionality" "C#"
                dataIntegrationModule = component "Data Integration Module" "Provides data integration functionality" "C# + TypeScript" 
                mapperLibrary = component "Mapper Library" "Provides mapping functionality" "TypeScript + JointJS"
                notificationModule = component "Notification Module" "Provides notification functionality" "C#"
                metabaseDeploymentModule = component "Metabase Deployment Module" "Manages Kubernetes deployment" "C#: "


                blazorServer -> userManagementModule "Executes user related requests using"
                blazorServer -> dataIntegrationModule "Executes data integration requests using"
                blazorServer -> notificationModule "Executes notification requests using"
                blazorServer -> metabaseDeploymentModule "Executes deployment requests using"

                metabaseDeploymentModule -> k8sCluster.k8sAPI "Deploys database and Metabase instances"

                userManagementModule -> notificationModule "Sends notifications to users via"
                metabaseDeploymentModule -> notificationModule "Sends notifications to users via"
                # dataIntegrationModule -> mediator "Subscribes and publishes data integration events"
                # metabaseDeploymentModule -> mediator "Subscribes and publishes deployment events"
                dataIntegrationModule -> mapperLibrary "Integrates"
                dataIntegrationModule -> metabaseDeploymentModule "Provides mapping for deployment"

                notificationModule -> email "Sends e-mails to users via" "SMTP"


            }

            
            projectDatabase = container "BI Management Database" "Provides data persistence for BI projects" "MS SQL" {
                tags "Database"
            }

            // relations
            managementApp -> projectDatabase "Uses"
            managementApp.dataIntegrationModule -> projectDatabase "Saves data mappings"
            managementApp.userManagementModule -> projectDatabase "Saves user information"
            managementApp.metabaseDeploymentModule -> projectDatabase "Saves project statuses"
        }
        

        // relations 
        costumer -> biManagementSystem.managementHtml "Maps his data"
        administrator -> biManagementSystem.managementHtml "Manages BI projects and costumers"

        deploymentEnvironment "Live" {
            usersComputer = deploymentNode "User's Computer" {
                webBrowser = deploymentNode "Web Browser" {
                    liveManagementHtml = containerInstance biManagementSystem.managementHtml "Live Management HTML" "Manages BI projects in web browser"
                }
            }

            kubernetesNode = deploymentNode "Kubernetes Cluster" {
                tags "k8s cluster"
                metabaseNode = deploymentNode "Metabase Instance" n {
                    liveMetabaseInstance = containerInstance k8sCluster.metabaseInstance "Live Metabase Instance" "Metabase instance for costumer"
                }

                gatewayNode = deploymentNode "Router" {
                    tags "Kubernetes - pod"
                    liveRouter = infrastructureNode "Router" "Nginx ingress controller" {
                        tags "Kubernetes - ing"
                        description "Routes incoming requests based on URL path"
                    }
                }

                gatewayNode.liveRouter -> metabaseNode.liveMetabaseInstance "Routes requests to"

                kubeApiNode = deploymentNode "Kubernetes API" {
                    liveK8sAPI = containerInstance k8sCluster.k8sAPI "Live Kubernetes API" "API for managing Kubernetes cluster"
                }

                liveServerNode = deploymentNode "Backend Server" {
                    tags "Kubernetes - pod"
                    liveManagementApp = containerInstance biManagementSystem.managementApp "Management App" "Manages BI projects"
                }

                liveDBNode = deploymentNode "Database" {
                    tags "Kubernetes - pod"
                    liveDatabase = containerInstance biManagementSystem.projectDatabase "Live Database" "Database for Metabase"
                }

                gatewayNode.liveRouter -> liveServerNode.liveManagementApp "Routes requests to"

                k8sClusterNode = deploymentNode "k8sCluster" {
                    tags "External"
                    liveCluster = softwareSystemInstance k8sCluster "E-mail Server" "SMTP server"
                }

                gatewayNode.liveRouter -> k8sClusterNode.liveCluster "Routes Metabase traffic to"
            }

            deploymentNode "E-mail Server" {
                tags "External"
                liveEmail = softwareSystemInstance email "E-mail Server" "SMTP server"
            }

            usersComputer.webBrowser.liveManagementHtml -> kubernetesNode.gatewayNode.liveRouter "Sends requests to"
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
            # autoLayout
            include *
        }

        component biManagementSystem.managementApp "biManagementSystemManagementApiComponentDiagram" {
            # autoLayout
            include *
        }

        deployment biManagementSystem "Live" "liveDeploymentDiagram" {
            include *
            exclude "relationship.tag==excludeInDeployment"
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

            element "k8s cluster" {
                icon "https://static-00.iconduck.com/assets.00/kubernetes-icon-2048x1995-r1q3f8n7.png"
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

        themes https://static.structurizr.com/themes/kubernetes-v0.3/theme.json
    }
}




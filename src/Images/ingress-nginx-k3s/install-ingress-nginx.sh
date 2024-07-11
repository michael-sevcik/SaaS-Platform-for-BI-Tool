#!/bin/sh

# function to check the status of the Ingress NGINX deployment
check_ingress_nginx() {
  kubectl get pods -n ingress-nginx -l app.kubernetes.io/component=controller
}

# wait for K3s to be up and running
until kubectl get nodes; do
  echo "Waiting for K3s to start..."
  sleep 5
done

# apply the Ingress NGINX manifest
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/cloud/deploy.yaml

# wait for Ingress NGINX to be up and running
echo "Waiting for Ingress NGINX to start..."
until check_ingress_nginx | grep -q "Running"; do
  echo "Waiting for Ingress NGINX pods to be in running state..."
  sleep 5
done

sleep 70
echo "Ingress NGINX is running successfully!"

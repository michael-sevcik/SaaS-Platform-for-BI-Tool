!/bin/sh

until kubectl get nodes; do
  echo "Waiting for K3s to start..."
  sleep 5
done

# apply the Ingress NGINX manifest
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/cloud/deploy.yaml

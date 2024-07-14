## Building the image
```
docker build -t ingress-nginx-k3s:latest .
```

## Running the container

```
docker run -d --name k3s-server --privileged --restart=unless-stopped -p 6443:6443 -p 8443:8443 -p 80:80 -p 443:443 ingress-nginx-k3s:latest
```

## connecting to the server
```
docker exec -it k3s-server sh
```
#!/bin/sh
# start the k3s server in the background
k3s "$@" &

# and run the install-ingress-nginx.sh script
/usr/local/bin/install-ingress-nginx.sh
wait

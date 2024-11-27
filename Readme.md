## Compile: 
docker build -t basket.service:v2.0 -f Basket.Service\Dockerfile .

docker build -t order.service:v1.0 -f Order.Service\Dockerfile .

## Run: 
docker run -it --rm -p 8000:8080 -e RabbitMq__HostName=host.docker.internal basket.service:v2.0

docker run -it --rm -p 8001:8080 -e RabbitMq__HostName=host.docker.internal order.service:v1.0
- Create your .Net project

`dotnet new webapi -n myapi`

- Build your .Net project

`dotnet build mywebapi.csproj`

- Run the application without Docker.

`docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo`

- Create your docker image

`docker build -t catalog:v1 . `

- mongodb needs to talk with the catalog container, so create a network for both

`docker network create sample`

- Start the mongo image in the same network as the catalog. First is the name of the image, in this case it is mongo and the second name is the actual image that yuou will use. 

`docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db --network=nettutorial mongo`

`docker run -d --rm --name mongodbmylibrary -p 27010:27017 -v mongodbdata:/data/db --network=mylibrary mongo`

- Start the catalog

`docker run -it --rm -p 8085:80 -e MongoDbSettings:Host=mongo --network=nettutorial catalog:v3`

`docker run -it --rm -p 8085:80 -e MongoDbSettings:Host=mongodbmylibrary --network=mylibrary mylibrary:v2`

`docker run -it --rm -p 8085:80 -e MongoDbSettings:Host=mongodbmylibrary -e MongoDbSettings:Port=27017 --network=mylibrary mylibrary:v3`

I had to specifically use the port 27017 on the new API because that is the port from the container. 

- Create the image for docker hub, so Kubernetes can use it

`docker build -t alexbr9007/catalog:v2 . `

- Submit your image to Docker Hub

`docker push alexbr9007/csharp_catalog:v2`

After you have pushed your image, you need to update the image in your catalog.yml file to run the latest image and perform a `kubectl apply -f catalog.yml`

- Create the name of the api in Kubernetes and the mongo db

`kubectl apply -f catalog.yml`
`kubectl apply -f mongodb.yml`

- Get all the current deployments

`kubectl get deployments`

- Get all the statefulsets

`kubectl get statefulsets`

- Get the logs from kubernetes

`kubectl logs <instance>`

- Get all the pods

`kubectl get pods -w`

- Scale up the kubernetes cluster

`kubectl scale deployments/catalog-deployment --replicas=2`

- Create unit tests

`dotnet new xunit -n UnitTests`

- Add a reference to the MYWEBAPI project from the UnitTests

`dotnet add reference ../MYWEBAPI/mywebapi.csproj`

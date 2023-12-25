- Build your .Net project

`dotnet build mywebapi.csproj`

- Run the application without Docker.

`docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo`

- Create your docker image

`docker build -t catalog:v1 . `

- mongodb needs to talk with the catalog container, so create a network for both

`docker network create sample`

- Start the mongo image in the same network as the catalog

`docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db --network=nettutorial mongo`

- Start the catalog

`docker run -it --rm -p 8085:80 -e MongoDbSettings:Host=mongo --network=nettutorial catalog:v3`

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
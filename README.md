- Run the application without Docker.

docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo

- Create your docker image

docker build -t catalog:v1 . 

- mongodb needs to talk with the catalog container, so create a network for both

docker network create sample

- Start the mongo image in the same network as the catalog

docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db --network=nettutorial mongo

- Start the catalog

docker run -it --rm -p 8085:80 -e MongoDbSettings:Host=mongo --network=nettutorial catalog:v3
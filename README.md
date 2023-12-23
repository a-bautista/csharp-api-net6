- Run the application without Docker.

docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo

- Create your docker image

docker build -t catalog:v1 . 

- mongodb needs to talk with the catalog container, so make both to join the network

docker network create sample

- Make the mongo and catalog images run in the same network

docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo --network=nettutorial catalog:v1


docker run -it --rm -p 8085:80 -e MongoDbSettings:Host=mongo --network=nettutorial catalog:v3
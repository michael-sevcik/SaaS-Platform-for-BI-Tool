# Choose ubuntu version
FROM mcr.microsoft.com/mssql/server:latest

# Create app directory
WORKDIR /usr/src/app

# Copy initialization scripts
COPY . /usr/src/app

USER root

RUN chmod +x ./entrypoint.sh
RUN chmod +x ./run-initialization.sh
             
# Set environment variables, not have to write them with the docker run command
# Note: make sure that your password matches what is in the run-initialization script 
ENV SA_PASSWORD=password123!
ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Express

# Expose port 1433 in case accessing from other container
# Expose port externally from docker-compose.yml
EXPOSE 1433

# Run Microsoft SQL Server and initialization script (at the same time)
CMD /bin/bash ./entrypoint.sh



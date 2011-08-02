Zed is a utility made to help manage environment configuration during deployment and afterwards.

### Purpose
1. The purpose is to have Zed track a certain deployment directory on a server, manage environment configuration settings, automatically locate and verify dependencies.
2. In the world of .net, for example, the following will be initially included :  
 * Database connectionstrings  
 * Msmq queues  
 * Urls of WCF Endpoints and/or other api datapoints
(...more with plugins/configuration/next versions)

3. Thoughts of Additional dependencies
 * Dlls/runtimes
 * IIS version compatibility
 * Socket/port binds usage

### Initial Goals
* Automatic mapping of projects, their dependencies and relationships using zero-configuration scanning of the deployment directory.
* Setup of initial dependency store and comparison to store on each deployment to detect new dependencies
* UI display of projects graph
* Centralized and per project views of configured dependencies (see 2.)

### Thoughts of features
* Tray icon alerting unknown/new dependencies deployed (rescan on directoryChange events)
* One click deploy of service - Zed could provide dependency verification, in addition to queue creation, service or IIS site installation (autodetect framework version according to config)
* process watchdog -> notifications / nagios integration
* web UI
* custom actions on component types (example : toggle WCF tracing on dot net config file)

### Need help with 
* Creating a website for the project
* everything in "Thoughts of features"
* suggestions and ideas
* documentation

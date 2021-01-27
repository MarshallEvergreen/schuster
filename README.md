[![Build status](https://dev.azure.com/MarshallEvergreen/schuster/_apis/build/status/schuster%20-%20CI)](https://dev.azure.com/MarshallEvergreen/schuster/_build/latest?definitionId=1)

# schuster
Schuster aims to provide an easy way to create pipelines in lua that consist of small tasks which execute in sequential order.
A pipeline can: 
* be the user Run
* be Aborted by the user
* report the progress as a percentage
* report the status of itself


The user can decide:
* when a task suceeds or errors
* define a function to excute when the task errors / aborts. 

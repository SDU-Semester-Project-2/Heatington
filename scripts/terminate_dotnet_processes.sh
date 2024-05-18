#!/bin/bash

# Find the PIDs of all dotnet processes
PIDS=$(ps -ax | grep dotnet | grep -v grep | awk '{print $1}')

if [ -z "$PIDS" ]; then
    echo "No dotnet processes found."
else
    # Send Ctrl + C signal to each process
    for PID in $PIDS
    do
        kill -2 $PID
        echo "Sent Ctrl + C signal to process with PID: $PID"
        kill $PID
        echo "Killed process: $PID"
    done
fi

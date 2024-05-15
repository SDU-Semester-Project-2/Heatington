#!/bin/bash

DEV=false
RUN_IN_TERMINAL_CMD=alacritty -e
# RUN_IN_TERMINAL_CMD=x-terminal-emulator -e

CUR_DIR := $(shell pwd)

ifeq ($(OS),Windows_NT)
    RUN_IN_TERMINAL := start cmd /k
else ifeq ($(shell uname),Darwin)
    RUN_IN_TERMINAL := open -a Terminal
else ifeq ($(shell cat /etc/os-release 2>/dev/null | grep -c nixos),1)
    RUN_IN_TERMINAL := x-terminal-emulator -e
else
    RUN_IN_TERMINAL := $(RUN_IN_TERMINAL_CMD)
endif

ifeq ($(DEV),true)
    DOTNET_RUN_CMD := dotnet watch
else
    DOTNET_RUN_CMD := dotnet run
endif

run:
	@echo "Starting all services..." &
	@make api &
	@make web &
#    @make console &
	@echo "end..."

console:
	@echo "Running Console app..."
	$(RUN_IN_TERMINAL) sh -c "cd $(CUR_DIR)/Heatington.Console && $(DOTNET_RUN_CMD)"
	@echo "Console app terminated"

web:
	@echo "Running Web app..."
	$(RUN_IN_TERMINAL) sh -c "cd $(CUR_DIR)/Heatington.Web && $(DOTNET_RUN_CMD)"
	@echo "Web app is terminated"

api:
	@echo "Running APIs..."
	$(RUN_IN_TERMINAL) sh -c "cd $(CUR_DIR)/Heatington.Microservice.OPT && $(DOTNET_RUN_CMD)"
	$(RUN_IN_TERMINAL) sh -c "cd $(CUR_DIR)/Heatington.Microservice.AM && $(DOTNET_RUN_CMD)"
	$(RUN_IN_TERMINAL) sh -c "cd $(CUR_DIR)/Heatington.Microservice.SDM && $(DOTNET_RUN_CMD)"
	$(RUN_IN_TERMINAL) sh -c "cd $(CUR_DIR)/Heatington.Microservice.RDM && $(DOTNET_RUN_CMD)"
	@echo "APIs terminated!"

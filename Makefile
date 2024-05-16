DEV=true

# Determine the operating system
ifeq ($(OS),Windows_NT)
    OS_TYPE := Windows
else
    OS_TYPE := $(shell uname)
endif

# Define the default target
.DEFAULT_GOAL := run

# Define the run_projects target
run:
ifeq ($(OS_TYPE),Windows)
	@echo Running on Windows
	@$(MAKE) run_web_windows
else
	@echo Running on Unix-based system
	@$(MAKE) run_web_unix
endif

run_console:
ifeq ($(OS_TYPE),Windows)
	@echo Running on Windows
	@$(MAKE) run_console_windows
else
	@echo Running on Unix-based system
	@$(MAKE) run_console_unix
endif

#### PRODUCTION ####

# Define the run_web_windows target
run_web_windows:
ifeq ($(DEV),true)
	@$(MAKE) run_web_windows_dev
else:
	@echo Running web project on Windows
	@call ./scripts/run_web.bat
endif

# Define the run_web_unix target
run_web_unix:
ifeq ($(DEV),true)
	@$(MAKE) run_web_unix_dev
else:
	@echo Running web project on Unix-based system
	@chmod +x ./scripts/run_web.sh
	@./scripts/run_web.sh
endif

# Define the run_console_windows target
run_console_windows:
	@echo Running console project on Windows
	@call ./scripts/run_console.bat

# Define the run_console_unix target
run_console_unix:
	@echo Running console project on Unix-based system
	@chmod +x ./scripts/run_console.sh
	@./scripts/run_console.sh

#### DEV ####

# Define the run_web_windows target
run_web_windows_dev:
	@echo Running web project on Windows
	@call ./scripts/run_web_dev.bat

# Define the run_web_unix target
run_web_unix_dev:
	@echo Running web project on Unix-based system
	@chmod +x ./scripts/run_web_dev.sh
	@./scripts/run_web_dev.sh

# Define the run_console_windows target
run_console_windows_dev:
	@echo Running console project on Windows
	@call ./scripts/run_console_dev.bat

# Define the run_console_unix target
run_console_unix_dev:
	@echo Running console project on Unix-based system
	@chmod +x ./scripts/run_console_dev.sh
	@./scripts/run_console_dev.sh



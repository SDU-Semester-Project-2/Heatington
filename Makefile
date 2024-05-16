# Determine the operating system
ifeq ($(OS),Windows_NT)
    OS_TYPE := Windows
else
    OS_TYPE := $(shell uname)
endif

# Define the default target
.DEFAULT_GOAL := run_projects

# Define the run_projects target
run_projects:
ifeq ($(OS_TYPE),Windows)
	@echo Running on Windows
	@$(MAKE) run_projects_windows
else
	@echo Running on Unix-based system
	@$(MAKE) run_projects_unix
endif

# Define the run_projects_windows target
run_projects_windows: run_web_windows run_console_windows

# Define the run_projects_unix target
run_projects_unix: run_web_unix run_console_unix

# Define the run_web_windows target
run_web_windows:
	@echo Running web project on Windows
	@call ./scripts/run_web.bat

# Define the run_web_unix target
run_web_unix:
	@echo Running web project on Unix-based system
	@./scripts/run_web.sh

# Define the run_console_windows target
run_console_windows:
	@echo Running console project on Windows
	@call ./scripts/run_console.bat

# Define the run_console_unix target
run_console_unix:
	@echo Running console project on Unix-based system
	@./scripts/run_console.sh

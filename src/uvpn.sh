#!/bin/bash

api="https://api.uvpn.me"
user_agent="Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36"

function login() {
	response=$(curl --request POST \
		--url "$api/v2/user/create" \
		--user-agent "$user_agent" \
		--header "content-type: application/json" \
		--header "datatype: json" \
		--data '{
			"version": "7.1.4",
			"platform": "Windows",
			"browser": "chrome",
			"browser_lang": "ru-RU",
			"type": "browser-extension",
			"user_version": "UVPNv3"
		}')
	if [ -n $(jq -r ".data.auth_token" <<< "$response") ]; then
		auth_token=$(jq -r ".data.auth_token" <<< "$response")
		device_token=$(jq -r ".data.device_token" <<< "$response")
		proxy_token=$(jq -r ".data.proxy_token" <<< "$response")
		url_token=$(jq -r ".data.url_token" <<< "$response")
	fi
	echo $response
}

function get_servers() {
	curl --request POST \
		--url "$api/v2/servers/ping" \
		--user-agent "$user_agent" \
		--header "content-type: application/json" \
		--header "datatype: json" \
		--header "authorization: $auth_token" \
		--data '{
			"device_token": "'$device_token'"
		}'
}

function get_account_info() {
	curl --request POST \
		--url "$api/user/info" \
		--user-agent "$user_agent" \
		--header "content-type: application/json" \
		--header "datatype: json" \
		--header "authorization: $auth_token"
}


function get_notifications() {
	curl --request POST \
		--url "$api/notifications/list" \
		--user-agent "$user_agent" \
		--header "content-type: application/json" \
		--header "datatype: json" \
		--header "authorization: $auth_token"
}

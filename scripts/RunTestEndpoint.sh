#!/bin/bash

ENDPOINT_IDENTIFIER=$(uuidgen)
PORT=$1
ENDPOINT_NAME=$2
SAVED_EVENTS_PATH=$3

function log_message() {
    MESSAGE=$1
    LOG_LEVEL=$2

    echo $MESSAGE

    curl \
        -X POST "http://localhost:${PORT}/api/endpoints/${ENDPOINT_IDENTIFIER}/logs" \
        -H "Content-Type: application/json" \
        -d "{
            \"logLevel\": ${LOG_LEVEL},
            \"message\": \"${MESSAGE}\",
            \"data\": { }
        }"
}

curl \
    -X POST "http://localhost:${PORT}/api/endpoints/" \
    -H "Content-Type: application/json" \
    -d "{
        \"identifier\": \"$ENDPOINT_IDENTIFIER\",
        \"name\": \"$ENDPOINT_NAME\",
        \"savedEventPath\": \"$SAVED_EVENTS_PATH\",
        \"columns\": [
            { \"title\": \"Detail Type\", \"pointer\": \"/detail_type\" }
        ]
    }"

log_message "Endpoint registered with id $ENDPOINT_IDENTIFIER" 2

while true
do
    EVENT=$(curl -s "http://localhost:${PORT}/api/endpoints/${ENDPOINT_IDENTIFIER}/events")
    EVENT_IDENTIFIER=$(echo $EVENT | jq -r '.identifier')

    if [ "$EVENT_IDENTIFIER" != "null" ]
    then
        IS_SUCCESS=$(echo $EVENT | jq -r '.data.detail.isSuccess')
        STATUS_MESSAGE=$(echo $EVENT | jq -r '.data.detail.message')

        log_message "Event found: $EVENT_IDENTIFIER" 1
        sleep 2
        curl \
            -X PUT "http://localhost:${PORT}/api/events/${EVENT_IDENTIFIER}" \
            -H "Content-Type: application/json" \
            -d "{
                \"isSuccess\": ${IS_SUCCESS},
                \"message\": \"${STATUS_MESSAGE}\"
            }"
    else
        log_message "No event received" 2
    fi

done


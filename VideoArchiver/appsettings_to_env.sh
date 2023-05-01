#!/bin/bash

declare -A env

function convert_value_to_env {
    local prefix=$1
    local value=$2
    local type=$(echo $value | jq -r '. | type')

    case $type in
    "array")
        convert_array_to_env $prefix "$value"
        ;;
    "object")
        convert_subobject_to_env $prefix "$value"
        ;;
    "number")
        env[$prefix]=$value
        ;;
    "boolean")
        env[$prefix]=$value
        ;;
    "null")
        env[$prefix]=""
        ;;
    *)
        if [[ $value =~ " " ]]; then
            env[$prefix]="\"$value\""
        else
            env[$prefix]=$value
        fi
        ;;
    esac
}

function convert_array_to_env {
    local prefix=$1
    local array=$2
    local index=0

    for item in $(echo $array | jq -r '.[] | @base64'); do
        convert_value_to_env "${prefix}__$index" "$(echo $item | base64 -d)"
        index=$((index + 1))
    done
}

function convert_subobject_to_env {
    local prefix=$1
    local object=$2

    for key in $(echo $object | jq -r '. | keys[]'); do
        convert_value_to_env "${prefix}__$key" "$(echo $object | jq -r ".[\"$key\"]")"
    done
}

jsonFile="${1:-WebApp/appsettings.json}"
for key in $(jq -r '. | keys[]' $jsonFile); do
    convert_value_to_env $key "$(jq -r ".[\"$key\"]" $jsonFile)"
done

for key in "${!env[@]}"; do
    echo "${key^^}=${env[$key]}"
done

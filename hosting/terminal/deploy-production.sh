#!/bin/sh

dotnet deploy -cloud:aliyun -site:daemon -edition:Debug -debug:off -environment:production -framework:net8.0

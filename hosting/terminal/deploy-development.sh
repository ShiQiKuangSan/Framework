#!/bin/sh

dotnet deploy -cloud:aliyun -site:daemon -edition:Debug -debug:off -environment:development -framework:net8.0

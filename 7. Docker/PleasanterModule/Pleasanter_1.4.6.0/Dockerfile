ARG VERSION=${PLEASANTER_VER}
FROM implem/pleasanter:${VERSION}

COPY Parameters/ App_Data/Parameters/
ENTRYPOINT [ "dotnet", "Implem.Pleasanter.dll" ]
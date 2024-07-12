ARG VERSION=${PLEASANTER_VER}
FROM implem/pleasanter:${VERSION}

COPY Parameters/ App_Data/Parameters/
COPY Implem.License.dll .
ENTRYPOINT [ "dotnet", "Implem.Pleasanter.dll" ]
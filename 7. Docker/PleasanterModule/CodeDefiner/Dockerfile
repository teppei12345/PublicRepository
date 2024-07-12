FROM implem/pleasanter:codedefiner

COPY Parameters/ /app/Implem.Pleasanter/App_Data/Parameters/
COPY Implem.License.dll .
ENTRYPOINT [ "dotnet", "Implem.CodeDefiner.dll" ]
# Docker Compose を利用した Pleasanter * PostgreSQL 環境構築用モジュール

## 概要

本モジュールでは、Docker 環境下で Pleasanter * PostgreSQL の運用ができます。  
各ツール・サービスについては下記をご参照ください。  

* [Pleasanter](https://pleasanter.org/purpose)  
* [PostgreSQL](https://www.postgresql.jp/document/)  
* [Docker](https://www.docker.com/)  
* [Docker Compose](https://github.com/docker/compose)  

### 対象者

* Docker を利用した Pleasanter にてバージョンアッ等によるパラメータ設定の引継ぎ方法に悩まれている方  
* Docker を利用した PostgreSQL にて蓄積したデータを永続化したい方  
* 上記どちらも実現されたい方  
* Docker を利用した Pleasanter * PostgreSQL の環境構築を目指されている方  
* Pleasanterを快適に利用されたい方  

### できること

* 4つのコマンドで Pleasanter の利用ができます  
* Pleasanter のパラメータ設定を引き継ぐことができます  
* PostgreSQL に蓄積したデータを永続化できます  

### 内容

本モジュールには、下記ツールの下記バージョンが含まれます。  

* Pleasanter：1.3.6.0  
* PostgreSQL：15  
* pgadmin：8.6  

## 導入手順

1. 同階層に配置されている「PleasanterModule.zip」をダウンロード  
2. 「PleasanterModule.zip」を任意のディレクトリに解凍  
    **※ 解凍するディレクトリに指定はありません**  
3. 「PleasanterModule」ディレクト上でコマンドプロンプトを起動  
4. 
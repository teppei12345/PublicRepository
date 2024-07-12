# Docker Compose を利用した Pleasanter * PostgreSQL 環境構築用モジュール

## 概要

本モジュールでは、Docker 環境下で Pleasanter * PostgreSQL の運用ができます。  
各ツール・サービスについては下記をご参照ください。  

* [Pleasanter](https://pleasanter.org/purpose)  
* [PostgreSQL](https://www.postgresql.jp/document/)  
* [Docker](https://www.docker.com/)  
* [Docker Compose](https://github.com/docker/compose)  

### 対象者

* Docker を利用した Pleasanter にてバージョンアップ等によるパラメータ設定の引継ぎ方法に悩まれている方  
* Docker を利用した PostgreSQL にて蓄積したデータを永続化したい方  
* 上記どちらも実現されたい方  
* Docker を利用した Pleasanter * PostgreSQL の環境構築を目指されている方  
* Pleasanterを快適に利用されたい方  

### できること

* 3つのコマンドで Pleasanter の利用ができます  
* Pleasanter のパラメータ設定を引き継ぐことができます  
* PostgreSQL に蓄積したデータを永続化できます  

### 内容

本モジュールには、下記バージョンが含まれます。  

|ツール|バージョン|
|:----|:----|
|Pleasanter|1.3.6.0|
|PostgreSQL|15|
|pgadmin4|8.9|

## 手順

### 導入

1. 同階層に配置されている「PleasanterModule.zip」をダウンロード  
2. 「PleasanterModule.zip」を任意のディレクトリに解凍  
    **※ 解凍するディレクトリに指定はありません**  
3. 「PleasanterModule」ディレクト上にライセンスファイルを配置  
4. 「PleasanterModule」ディレクト上でコマンドプロンプトを起動  
5. 下記の順番の通り、コマンドを実行  

#### 1. イメージプル

```CMD
docker compose build
```

#### 2. PostgreSQLコンテナ実行とCodedefinerの実行

```CMD
docker compose run --rm codedefiner _rds
```

#### 3. Pleasanterコンテナ実行

```CMD
docker compose up -d pleasanter
```

#### 4. pgadmin4のイメージプル・コンテナ実行

PostgreSQLを参照するためのGUIツール「pgadmin4」をご利用の方はこちらのコマンドも実行してください。  

```CMD
docker compose up -d pgadmin4
```

### 起動・動作確認

各ツールが起動しているかどうか確認してください。  

#### Pleasanter(localhost:50001)にアクセス

[localhost:50001](http://localhost:50001/)にアクセスし、Pleasanter のログイン画面にアクセスできるか確認してください。  

#### Pleasanterでテーブルを作成

下記の手順で記録テーブルを作成し、一覧画面に遷移することを確認してください。  

##### 初回ログインからテーブル作成の手順

1. 下記「Pleasanterログイン情報」の通り入力後、「ログイン」をクリック  
2. 表示されたダイアログ上で任意のパスワードを入力し、「変更」をクリック  
    **※ 以降のログイン情報となるため、厳重に管理してください**  
3. 画面左上の「+」をクリック  
4. 遷移後の画面左から「記録テーブル」を選択後、「作成」をクリック  
5. 任意の名前を入力後、「作成」をクリック  
6. 作成されたテーブルをクリックし、一覧画面に遷移することを確認  

**Pleasanterログイン情報**  

|パラメータ|値|
|:----|:----|
|ログインID|Administrator|
|パスワード|pleasanter|

#### pgadmin4(localhost:12345)にアクセス

[localhost:12345](http://localhost:12345/)にアクセスし、pgadmin4 のログイン画面にアクセスできるか確認してください。  

#### pgadmin4でデータを確認

1. 下記「pgadmin4ログイン情報」の通り入力後、「Login」をクリック  
2. 「Servers」を右クリックし、「登録 > サーバ...」をクリック  
3. 下記「サーバ登録情報」の通り入力後、「適用」をクリック  
4. 「Servers > db > データベース > Implem.Pleasanter > スキーマ > Implem.Pleasanter > テーブル > Items」テーブルを右クリック  
5. 「データを閲覧/編集 > 最後の100行」をクリック  
6. 先ほど作成したテーブルのデータ(ReferenceId：1)の存在を確認  

**pgadmin4ログイン情報**  

|パラメータ|値|
|:----|:----|
|ログインID|example@example.co.jp|
|パスワード|password|

**サーバ登録情報**  

|パラメータ|値|
|:----|:----|
|名前|db|
|ホスト名/アドレス|db|
|ユーザ名|postgres|
|パスワード|postgres|

# �v���O�����̏������x���v��
$processingSpeed = (Measure-Command {

<#
*********************************************
�ϐ���`�ӏ�
���ɍ��킹�ēK�X�ݒ肵�Ă��������B
*********************************************
#>

<#
���O�t�@�C���Ɋւ���ϐ���`
#>

# ���O�t�@�C�����폜������Ԃ��`(���ɂ��P��)
# ��) 18�� 17:00�Ɏ��s���A���u4�v���w�肵���ꍇ�A14����16:59�ȑO�̃t�@�C�����폜����܂�
$deletLogPeriod = 4
# ���O�t�@�C���̊i�[�f�B���N�g����Path���`
$toPath = 'XXXX'

<#
�f�[�^�x�[�X�Ɋւ���ϐ���`
#>

# �f�[�^�x�[�X�����`
$databaseName = 'XXXX'
# Windows�����F�؂��`(TRUE:�L����, FALSE:������)
$integratedSecurity = 'XXXX'
# ���[�UID���`
$userId = 'XXXX'
# �p�X���[�h���`
$password = 'XXXX'

<#
SQL���Ɋւ���ϐ���`
#>

# AD�A�g���[�U��o�^����O���[�v���`
$groupId = 7
# �h���C�����`
$domain = 'XXX.XX'
# Creator�̒�`
$creator = 1
# Updator�̒�`
$updator = 1
# Admin�̒�`
$admin = 0



<#
*********************************************
���W�b�N�ӏ�
���������̓v���O�����̂��ߏC���s�v�ł��B
*********************************************
#>

<#
���O�t�@�C���Ɋւ���ϐ���`
#>

# ���ݓ������擾
$dateLogFile = Get-Date -Format "yyyyMMddHHmmss"
# ���O�t�@�C�������`
$fileName = 'log' + $dateLogFile
# ���O�t�@�C���̊i�[�ꏊ���`
$filePath = $toPath + '\' + $fileName + '.log'

<#
SQL���Ɋւ���ϐ���`
#>

# �g�DID�̒�`
$deptId = 0
# ver�̒�`
$ver = 0
# Comments�̒�`
$comments = ''
# SQL��3��LIKE��̒�`
$like = '%@' + $domain
try {
    # �w��̃t�H���_�����݂���ꍇ�̏���
    if (Test-Path $toPath) {

        # �����̃��O�t�@�C�������݂��Ȃ��ꍇ�̏���
        if ( - !(Test-Path $filePath)) {

            # ��̃��O�t�@�C����V�K�쐬
            Out-File $filePath

            # ���O�t�@�C�����쐬���ꂽ���Ƃ����O�t�@�C���ɏo��
            $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z���O�t�@�C�����쐬����܂����B�쐬�t�@�C���F' + $filePath
            Add-Content -Path $filePath -Value $comment

            # �X�N���v�g�����s���鎖�����O�t�@�C���ɏo��
            $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�X�N���v�g�̎��s�J�n'
            Add-Content -Path $filePath -Value $comment

        }
        # �����̃��O�t�@�C�������݂���ꍇ�̏���
        else {

            # ���O�t�@�C���ɒǋL����ꍇ�̃��O�J�n�ʒu���킩��悤�Ƀ��O�t�@�C���ɏo��
            $comment = '------------------------------------'
            Add-Content -Path $filePath -Value $comment

            # �X�N���v�g�����s���鎖�����O�t�@�C���ɏo��
            $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�X�N���v�g�̎��s�J�n'
            Add-Content -Path $filePath -Value $comment
        }
    }
    # �w��̃t�H���_�����݂��Ȃ��ꍇ�̏���
    else {

        # �w��̃t�H���_�����݂��Ȃ��|�A�R���\�[���ɏo��
        $comment = '�w��̃t�H���_�����݂��܂���B�w��̃t�H���_�F' + $toPath
        write-host $comment

        exit

    }

    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�t�@�C���̍폜�����J�n'
    Add-Content -Path $filePath -Value $comment

    # �ߋ��̃��O�t�@�C���̍폜�������J�n
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z�폜�Ώۃt�@�C���F��������' + $deletLogPeriod + '���ȑO�̃t�@�C��'
    Add-Content -Path $filePath -Value $comment

    # �ߋ��̃��O�t�@�C�����폜
    Get-ChildItem $toPath -Recurse | Where-Object { $_.CreationTime -lt (Get-Date).AddDays(-$deletLogPeriod) } | Remove-Item -Force

    # �ߋ��̃��O�t�@�C���̍폜�������I��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�t�@�C���̍폜�����I��'

    # ���O�t�@�C���̏o�͊J�n
    Add-Content -Path $filePath -Value $comment
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z���O�t�@�C���o�͊J�n'
    Add-Content -Path $filePath -Value $comment

    # �ڑ����̍쐬
    # �ڑ����ϐ��𐶐�
    $connectionString = New-Object -TypeName System.Data.SqlClient.SqlConnectionStringBuilder

    # �ڑ�������̐��������J�n
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�ڑ�������̐��������J�n'
    Add-Content -Path $filePath -Value $comment

    # �f�[�^�x�[�X����ݒ�
    $connectionString['Database'] = $databaseName

    # �f�[�^�x�[�X�������O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z�f�[�^�x�[�X�� �F' + $databaseName
    Add-Content -Path $filePath -Value $comment

    # Windows�����F�؂�ݒ�
    $connectionString['Integrated Security'] = $integratedSecurity

    # Windows�����F�؂����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�zWindows�����F�؁F' + $integratedSecurity
    Add-Content -Path $filePath -Value $comment

    # ���[�UID��ݒ�
    $connectionString['User ID'] = $userId

    # �p�X���[�h��ݒ�
    $connectionString['Password'] = $password

    # �ڑ�������̐��������I��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�ڑ�������̐��������I��'
    Add-Content -Path $filePath -Value $comment

    # SQL���̍쐬
    # SQL��1:�w�肳�ꂽ�O���[�v�̑��݉�
    $selectGroupQuery = "
        select Count(*) from [" + $databaseName + "].[dbo].[Groups] where [GroupId] = '${groupId}';
    "
    # SQL��2:�w�肳�ꂽ���[�U�̑��݉�
    $selectUserQuery = "
        select Count(*) from [" + $databaseName + "].[dbo].[MailAddresses]
               where [MailAddress] like '${like}'
               and [OwnerId] in
               (
                   select [UserId]
                   from [" + $databaseName + "].[dbo].[Users]
                   where [Disabled] = 0
               );
    "
    # SQL��3:����̃O���[�v�̃����o�[���ꊇ�폜����
    $deleteQuery = "
        delete from [" + $databaseName + "].[dbo].[GroupMembers] where [GroupId] = '${groupId}';
    "
    # SQL��4:�����łȂ��A�����[���A�h���X������̃h���C���̃��[�U�[���O���[�v�̃����o�[�ɑ}������
    $insertQuery = "
        insert into [" + $databaseName + "].[dbo].[GroupMembers]
        (
            [GroupId]
            ,[DeptId]
            ,[UserId]
            ,[Ver]
            ,[Admin]
            ,[Comments]
            ,[Creator]
            ,[Updator]
        )
            select DISTINCT
               '${groupId}'
               ,'${deptId}'
               ,[OwnerId]
               ,'${ver}'
               ,'${admin}'
               ,'${comments}'
               ,'${creator}'
               ,'${updator}'
               from [" + $databaseName + "].[dbo].[MailAddresses]
               where [MailAddress] like '${like}'
               and [OwnerId] in
               (
                   select [UserId]
                   from [" + $databaseName + "].[dbo].[Users]
                   where [Disabled] = 0
               );
    "
    # SQL�R�l�N�V�����̐ݒ�J�n
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�zSQL�R�l�N�V�����̐ݒ�J�n'
    Add-Content -Path $filePath -Value $comment

    # �R�l�N�V�������̐ݒ�
    $sqlConnection = New-Object System.Data.SQLClient.SQLConnection($connectionString)

    # �R�l�N�V�����������O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�zSQL�R�l�N�V�������ݒ肳��܂����B'
    Add-Content -Path $filePath -Value $comment

    # SQL�R�l�N�V�����̐ݒ�I��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�zSQL�R�l�N�V�����̐ݒ�I��'
    Add-Content -Path $filePath -Value $comment

    # SQL�R�}���h�̐ݒ�J�n
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�zSQL�R�}���h�̐ݒ�J�n'
    Add-Content -Path $filePath -Value $comment

    # SQL��1��ݒ�
    $selectGroupSqlCommand = New-Object System.Data.SQLClient.SQLCommand($selectGroupQuery, $sqlConnection)

    # SQL��1�����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�zGroups��SELECT���F' + $selectGroupQuery
    Add-Content -Path $filePath -Value $comment

    # SQL��2��ݒ�
    $selectUserSqlCommand = New-Object System.Data.SQLClient.SQLCommand($selectUserQuery, $sqlConnection)

    # SQL��2�����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�zMailAddress��SELECT���F' + $selectUserQuery
    Add-Content -Path $filePath -Value $comment

    # SQL��3��ݒ�
    $deleteSqlCommand = New-Object System.Data.SQLClient.SQLCommand($deleteQuery, $sqlConnection)

    # SQL��3�����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�zGroupMembers��DELETE���F' + $deleteQuery
    Add-Content -Path $filePath -Value $comment

    # SQL��4��ݒ�
    $insertSqlCommand = New-Object System.Data.SQLClient.SQLCommand($insertQuery, $sqlConnection)

    # SQL��4�����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�zGroupMembers��INSERT���F' + $insertQuery
    Add-Content -Path $filePath -Value $comment

    # SQL�R�}���h�̐ݒ�I��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�zSQL�R�}���h�̐ݒ�I��'
    Add-Content -Path $filePath -Value $comment

    # �f�[�^�x�[�X�̐ڑ������J�n
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�f�[�^�x�[�X�̐ڑ������J�n'
    Add-Content -Path $filePath -Value $comment

    # �f�[�^�x�[�X�ڑ�
    $sqlConnection.Open()

    # �f�[�^�x�[�X�ڑ��J�n�����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z�f�[�^�x�[�X�̐ڑ����J�n���܂����B'
    Add-Content -Path $filePath -Value $comment

    # �f�[�^�x�[�X�̐ڑ������I��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�f�[�^�x�[�X�̐ڑ������I��'
    Add-Content -Path $filePath -Value $comment

    # �g�����U�N�V�����̐ݒ菈���X�^�[�g
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�g�����U�N�V�����̐ݒ菈���J�n'
    Add-Content -Path $filePath -Value $comment

    # �g�����U�N�V�����̐ݒ�
    $transaction = $SqlConnection.BeginTransaction()

    # �g�����U�N�V�������J�n���ꂽ���Ƃ����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z�g�����U�N�V�������J�n����܂����B'
    Add-Content -Path $filePath -Value $comment

    # �g�����U�N�V�����̐ݒ菈���I��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�g�����U�N�V�����̐ݒ菈���I��'
    Add-Content -Path $filePath -Value $comment

    # �e�R�}���h�ւ̃g�����U�N�V�����̊��蓖�ď����J�n
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�g�����U�N�V�����̊��蓖�ď����J�n'
    Add-Content -Path $filePath -Value $comment

    # SQL��1�Ƀg�����U�N�V�����̊��蓖�Ă�ݒ�
    $selectGroupSqlCommand.Transaction = $transaction

    # SQL��2�Ƀg�����U�N�V�����̊��蓖�Ă�ݒ�
    $selectUserSqlCommand.Transaction = $transaction

    # SQL��3�Ƀg�����U�N�V�����̊��蓖�Ă�ݒ�
    $deleteSqlCommand.Transaction = $transaction

    # SQL��4�Ƀg�����U�N�V�����̊��蓖�Ă�ݒ�
    $insertSqlCommand.Transaction = $transaction

    # �g�����U�N�V���������蓖�Ă�ꂽ���Ƃ����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z�g�����U�N�V������SQL���Ɋ��蓖�Ă��܂����B'
    Add-Content -Path $filePath -Value $comment

    # �e�R�}���h�ւ̃g�����U�N�V�����̊��蓖�ď����I��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�g�����U�N�V�����̊��蓖�ďI��'
    Add-Content -Path $filePath -Value $comment

    try {
        # SQL��1�𔭍s
        $selectGroupCount = $selectGroupSqlCommand.ExecuteScalar()

        # Groups�e�[�u���Ɏw�肵���O���[�v�����݂��Ă��Ȃ��ꍇ
        if ($selectGroupCount -eq 0) {

            # �������I������|���O�ɏo��
            $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yERROR�z�w�肳�ꂽ�O���[�v�͑��݂��܂���B'
            Add-Content -Path $filePath -Value $comment

            throw '�yERROR�z�������O���[�v���w�肵�Ă��������B�w��̃O���[�vID�F' + $groupId

        }
        # Groups�e�[�u���Ɏw�肵���O���[�v�����݂��Ă���ꍇ
        else {

            # �����𑱍s����|���O�ɏo��
            $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z�w�肳�ꂽ�O���[�v�͑��݂��Ă��܂��B�w��̃O���[�vID�F' + $groupId
            Add-Content -Path $filePath -Value $comment

        }

        # SQL��2�𔭍s
        $selectUserCount = $selectUserSqlCommand.ExecuteScalar()

        # Groups�e�[�u���Ɏw�肵�����[�U�����݂��Ă��Ȃ��ꍇ
        if ($selectUserCount -eq 0) {

            # �������I������|���O�ɏo��
            $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yERROR�z�w�肳�ꂽ�h���C���������[�U�͑��݂��܂���B'
            Add-Content -Path $filePath -Value $comment

            throw '�yERROR�z�������h���C�����w�肵�Ă��������B�w��̃h���C���F@' + $domain

        }
        # MailAddress�e�[�u���Ɏw�肵���h���C���������[�U�����݂��Ă���ꍇ
        else {

            # �����𑱍s����|���O�ɏo��
            $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z�w�肳�ꂽ�h���C���������[�U�͑��݂��Ă��܂��B�w�肳�ꂽ�h���C���F@' + $domain
            Add-Content -Path $filePath -Value $comment

        }

        # SQL���s�J�n
        $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�zSQL���s�J�n'
        Add-Content -Path $filePath -Value $comment

        # �������J�n���邱�Ƃ����O�t�@�C���ɏo��
        $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z�������J�n���܂��B'
        Add-Content -Path $filePath -Value $comment

        # SQL��3�𔭍s
        $deleteCount = $deleteSqlCommand.ExecuteNonQuery()

        # SQL��3�̎��s���ʂ��o��
        $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�zGroupMembers��DELETE���̎��s���ʁF' + $deleteCount + '��'
        Add-Content -Path $filePath -Value $comment

        # SQL��4�𔭍s
        $insertCount = $insertSqlCommand.ExecuteNonQuery()

        # SQL��4�̎��s���ʂ��o��
        $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�zGroupMembers��INSERT���̎��s���ʁF' + $insertCount + '��'
        Add-Content -Path $filePath -Value $comment

        # �R�~�b�g�̎��s
        $transaction.Commit()

        # �������R�~�b�g���ꂽ���Ƃ����O�t�@�C���ɏo��
        $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z����������I�����܂����B'
        Add-Content -Path $filePath -Value $comment

        # SQL����I��
        $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�zSQL����I��'
        Add-Content -Path $filePath -Value $comment
    }
    catch {

        # ���[���o�b�N
        $transaction.Rollback()

        # �X�N���v�g�̏����ł̃G���[���e�����O�t�@�C���ɏo��
        $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] ' + $error[0]
        Add-Content -Path $filePath -Value $comment

        # ���������[���o�b�N���ꂽ���Ƃ����O�t�@�C���ɏo��
        $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yERROR�zSQL���̔��s�G���[�ɂ���Ĉُ�I�����܂����B'
        Add-Content -Path $filePath -Value $comment

        # SQL�ُ�I��
        $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�zSQL�ُ�I��'
        Add-Content -Path $filePath -Value $comment
    }
    finally {

        # �f�[�^�x�[�X�ڑ�����
        $SqlConnection.Close()

        # �t�@�C�������݂����ꍇ�̏���
        if (Test-Path $filePath) {

            # �f�[�^�x�[�X�ڑ��I�������O�t�@�C���ɏo��
            $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z�f�[�^�x�[�X�̐ڑ����������܂����B'
            Add-Content -Path $filePath -Value $comment

            # �f�[�^�x�[�X�̐ڑ��I��
            $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�f�[�^�x�[�X�̐ڑ��I��'
            Add-Content -Path $filePath -Value $comment

        }
    }
}
catch {

    # �X�N���v�g�̏����ł̃G���[���e�����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] ' + $error[0]
    Add-Content -Path $filePath -Value $comment

    # �X�N���v�g�̏����ŃG���[�������������Ƃ����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yERROR�z�F�X�N���v�g�̎��s�G���[�ɂ���Ĉُ�I�����܂����B'
    Add-Content -Path $filePath -Value $comment

    # �X�N���v�g�̎��s�G���[�ɂ���Ĉُ�I��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�X�N���v�g�ُ�I��'
    Add-Content -Path $filePath -Value $comment
}
finally {

    # �w��̃t�H���_�����݂��Ȃ��ꍇ�̏���
    if ( - !(Test-Path $toPath)) {

        exit
    }

    # �S�Ă̏������I���������Ƃ����O�t�@�C���ɏo��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yINFO�z�S�Ă̏������I�����܂����B'
    Add-Content -Path $filePath -Value $comment

    # �S�Ă̏������I��
    $comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�S�Ă̏����I��'
    Add-Content -Path $filePath -Value $comment
}
}).TotalMilliseconds

# �v���O�����̏������x�����O�t�@�C���ɏo��
$comment = '[' + $(Get-Date -Uformat %Y%m%d_%H:%M:%S) + '] �yDEBUG�z�������x(�~���b)' + $processingSpeed
Add-Content -Path $filePath -Value $comment
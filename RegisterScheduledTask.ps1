param (
    [Parameter(Mandatory = $true)]
    [string] $ExecuteFilePath
)

$currentUserId = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name

#
# Principal
#

$params = @{
    UserId    = $currentUserId
    LogonType = [Microsoft.PowerShell.Cmdletization.GeneratedTypes.ScheduledTask.LogonTypeEnum]::Interactive
    RunLevel  = [Microsoft.PowerShell.Cmdletization.GeneratedTypes.ScheduledTask.RunLevelEnum]::Limited
}
$principal = New-ScheduledTaskPrincipal @params

#
# Triggers
#
# TASK_SESSION_STATE_CHANGE_TYPE Enumeration
# https://docs.microsoft.com/en-us/windows/desktop/api/taskschd/ne-taskschd-task_session_state_change_type
#

$logonTrigger = New-ScheduledTaskTrigger -AtLogOn -User $currentUserId

$params = @{
    Namespace = 'ROOT\Microsoft\Windows\TaskScheduler'
    ClassName = 'MSFT_TaskSessionStateChangeTrigger'
}
$stateChangeTriggerClass = Get-CimClass @params

$params = @{
    CimClass = $stateChangeTriggerClass
    Property = @{
        UserId      = $currentUserId
        StateChange = 7  # TASK_SESSION_LOCK in taskschd.h
    }
    ClientOnly = $true
}
$lockTrigger = New-CimInstance @params

$params = @{
    CimClass = $stateChangeTriggerClass
    Property = @{
        UserId      = $currentUserId
        StateChange = 8  # TASK_SESSION_UNLOCK in taskschd.h
    }
    ClientOnly = $true
}
$unlockTrigger = New-CimInstance @params

#
# Action
#

$action = New-ScheduledTaskAction -Execute ('"{0}"' -f $ExecuteFilePath)

#
# Settings
#

$params = @{
    Disable                         = $false
    Hidden                          = $false
    Priority                        = 7
    Compatibility                   = [Microsoft.PowerShell.Cmdletization.GeneratedTypes.ScheduledTask.CompatibilityEnum]::Win8
    MultipleInstances               = [Microsoft.PowerShell.Cmdletization.GeneratedTypes.ScheduledTask.MultipleInstancesEnum]::IgnoreNew
    DisallowHardTerminate           = $false
    StartWhenAvailable              = $false
    RunOnlyIfNetworkAvailable       = $false
    DisallowDemandStart             = $false
    RunOnlyIfIdle                   = $false
    DisallowStartOnRemoteAppSession = $false
    WakeToRun                       = $false
    ExecutionTimeLimit              = New-TimeSpan -Hours 1
    RestartOnIdle                   = $false
    DontStopOnIdleEnd               = $false
    AllowStartIfOnBatteries         = $true
}
$settings = New-ScheduledTaskSettingsSet @params

#
# Scheduled task
#

$params = @{
    Description = 'Setting the desktop background image using the Windows Spotlight image.'
    Principal   = $principal
    Trigger     = $logonTrigger,$lockTrigger,$unlockTrigger
    Action      = $action
    Settings    = $settings
}
$task = New-ScheduledTask @params
$task.Author = $currentUserId
#$task.Date = (Get-Date).ToString('yyyy-MM-ddThh:mm:ss.fffffff')

#
# Register the new task.
#

$params = @{
    TaskName = 'WindowsSpotlightWallpaper'
    TaskPath = '\'
    User     = $currentUserId
    Force    = $true
}
$task | Register-ScheduledTask @params

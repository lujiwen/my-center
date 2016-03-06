### my-emegency-center 


###右键点出菜单，需要在xml文件写入类似下面的内容：
        <Grid.ContextMenu >
            <ContextMenu Background="#FF565656" BorderBrush="#FF565656" >
                <MenuItem Header="参数设置"  Background="#FF565656" Click="settings_btn_Click" Foreground="White" />
                <MenuItem Header="查看历史曲线" Background="#FF565656"  Click="history_btn_Click"  Foreground="White" BorderBrush="#FF565656" />
            </ContextMenu>
        </Grid.ContextMenu>
		
	然后具体点击事件，实现Click="some_event" 
	
	
 

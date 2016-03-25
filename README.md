# my-emegency-center


## 右键点出菜单，需要在xml文件写入类似下面的内容：

        <Grid.ContextMenu >
            <ContextMenu Background="#FF565656" BorderBrush="#FF565656" >
                <MenuItem Header="��������"  Background="#FF565656" Click="settings_btn_Click" Foreground="White" />
                <MenuItem Header="�鿴��ʷ����" Background="#FF565656"  Click="history_btn_Click"  Foreground="White" BorderBrush="#FF565656" />
            </ContextMenu>
        </Grid.ContextMenu>

	Ȼ�����������¼���ʵ��Click="some_event"


## 报表采用 WPF DataGrid
详见 http://www.cnblogs.com/xiaogangqq123/archive/2012/05/07/2487166.html

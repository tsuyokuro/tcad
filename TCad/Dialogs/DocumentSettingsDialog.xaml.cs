//#define DEFAULT_DATA_TYPE_DOUBLE
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


#if DEFAULT_DATA_TYPE_DOUBLE
using vcompo_t = System.Double;
using vector3_t = OpenTK.Mathematics.Vector3d;
using vector4_t = OpenTK.Mathematics.Vector4d;
using matrix4_t = OpenTK.Mathematics.Matrix4d;
#else
using vcompo_t = System.Single;
using vector3_t = OpenTK.Mathematics.Vector3;
using vector4_t = OpenTK.Mathematics.Vector4;
using matrix4_t = OpenTK.Mathematics.Matrix4;
#endif


namespace TCad.Dialogs;

/// <summary>
/// DocumentSettingsDialog.xaml の相互作用ロジック
/// </summary>
public partial class DocumentSettingsDialog : Window
{
    public vcompo_t WorldScale = (vcompo_t)(1.0);

    public DocumentSettingsDialog()
    {
        InitializeComponent();

        reduced_scale.PreviewTextInput += PreviewTextInputForNum;

        ok_button.Click += Ok_button_Click;
        cancel_button.Click += Cancel_button_Click;

        Loaded += DocumentSettingsDialog_Loaded;

        PreviewKeyDown += DocumentSettingsDialog_PreviewKeyDown;
    }

    private void DocumentSettingsDialog_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            HandleOK();
        }
        else if (e.Key == Key.Escape)
        {
            HandleCancel();
        }
    }

    private void DocumentSettingsDialog_Loaded(object sender, RoutedEventArgs e)
    {
        reduced_scale.Text = WorldScale.ToString();
    }

    private void Cancel_button_Click(object sender, RoutedEventArgs e)
    {
        HandleCancel();
    }

    private void Ok_button_Click(object sender, RoutedEventArgs e)
    {
        HandleOK();
    }

    private void HandleOK()
    {
        bool ret;
        vcompo_t v;

        ret = vcompo_t.TryParse(reduced_scale.Text, out v);

        WorldScale = v;

        DialogResult = ret;
    }

    private void HandleCancel()
    {
        DialogResult = false;
    }

    private void PreviewTextInputForNum(object sender, TextCompositionEventArgs e)
    {
        bool ok = false;

        TextBox tb = (TextBox)sender;

        vcompo_t v;
        var tmp = tb.Text + e.Text;
        ok = vcompo_t.TryParse(tmp, out v);

        e.Handled = !ok;
    }
}

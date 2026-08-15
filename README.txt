DeskClear

配布名：ウィンドウ全部閉じール

Windows 上で開いている通常ウィンドウへ WM_CLOSE を送り、まとめて終了要求を出す軽量ユーティリティです。

愛称、サンダーボルト⚡

概要

DeskClear は、複数の通常ウィンドウをまとめて閉じたいときのための小さな Windows ツールです。

強制終了ツールではありません。

Process.Kill() や TerminateProcess() は使わず、Windows の通常の終了要求である WM_CLOSE を各対象ウィンドウへ送ります。

そのため、終了時の保存確認・キャンセル・AutoSave・セッション復元などの挙動は、各アプリケーション側の実装に依存します。

主な特徴

・通常の可視トップレベルウィンドウを列挙
・対象へ WM_CLOSE を送信
・強制 Kill は行わない
・Windows Shell / Desktop などを明示的に保護
・Tool Window を除外
・DeskClear 自身を除外
・TextInputHost を保護
・本番と同じ対象判定を使う Dry Run を用意
・Quick 版は GUI / コンソールを表示せず即実行

プロジェクト構成

DeskClear/
│
├─ DeskClear.slnx
│
├─ DeskClear/
│  └─ WinForms版
│
├─ DeskClear.Core/
│  ├─ WindowCloser.cs
│  └─ WindowTargetInfo.cs
│
├─ DeskClear.Quick/
│  └─ 無GUI即実行版
│
└─ DeskClear.DryRun/
   └─ 診断用Dry Run

DeskClear

WinForms 版です。

現在は薄い UI から DeskClear.Core の処理を呼び出します。

DeskClear.Core

共通の中核処理です。

ウィンドウ列挙、安全判定、Win32 API 連携、WM_CLOSE 送信、Dry Run 用の診断情報生成を担当します。

DeskClear.Quick

普段使い用の本番実行版です。

起動すると WindowCloser.CloseAll() を呼び、そのまま終了します。

DeskClear.DryRun

診断用プロジェクトです。

通称：

サンダーボルト照準確認モード

本番と同じ対象判定を使いますが、WM_CLOSE は送信せず、対象ウィンドウの情報だけを表示します。

対象判定

v1.0 では、以下を対象外としています。

・GetShellWindow() で取得した Windows Shell
・GetDesktopWindow() で取得した Desktop Window
・非表示ウィンドウ
・Shell_TrayWnd
・Shell_SecondaryTrayWnd
・WS_EX_TOOLWINDOW を持つ Tool Window
・タイトルのないウィンドウ
・DeskClear 自身のプロセス
・TextInputHost

残った通常ウィンドウへ WM_CLOSE を送ります。

安全性について

DeskClear は、ファイルの保存・破棄・キャンセルを独自に判断しません。

WM_CLOSE を受け取った後の挙動は各アプリケーションに委ねます。

そのため、

・保存確認が表示される
・AutoSave により確認なしで終了する
・セッション復元を前提として確認なしで終了する
・キャンセルにより終了しない

など、アプリケーションによって挙動が異なります。

また、Windows 側の権限制約などにより、一部のウィンドウが閉じない場合があります。

「すべてのウィンドウを必ず閉じる」ことを保証するツールではありません。

開発時の安全確認

GitHub 公開前には、複数の AI によるコードレビューだけで判断せず、Dry Run を追加して Windows 実機上で対象ウィンドウを観測しました。

AIレビュー
    ↓
懸念点を仮説化
    ↓
Dry Runで照準対象を観測
    ↓
実機テスト
    ↓
安全判定へ反映

この過程で Tool Window や TextInputHost などを確認し、v1.0 の除外判定へ反映しています。

動作環境 / 配布方針

本番配布対象は DeskClear.Quick です。

現在の Publish 方針：

・Windows x64
・.NET 10
・Self-contained
・Single-file
・WinExe
・アプリケーションアイコン埋め込み

配布 exe 名：

ウィンドウ全部閉じール.exe

Self-contained 版では、別途 .NET ランタイムをインストールせずに実行できます。

使い方

1. Release から ウィンドウ全部閉じール.exe を取得します。
2. 閉じたい通常ウィンドウを開いた状態で exe を実行します。
3. 対象ウィンドウへ WM_CLOSE が送られます。
4. 保存確認などが表示された場合は、各アプリ上で判断してください。

ショートカットキーで使う場合

v1.0 本体にはグローバルホットキー機能を実装していません。

必要であれば Windows のショートカット機能を利用し、exe のショートカットへ任意のショートカットキーを設定できます。

ビルド

Visual Studio で DeskClear.slnx を開きます。

本番実行版は DeskClear.Quick です。

今後の候補

・ユーザーによる除外アプリ登録
・GUI 設定
・設定ファイル保存
・PostMessage() の結果ログ
・Dry Run の正式機能化
・タスクトレイ
・統合 Windows 効率化ツールへの組み込み

v1.0 では機能を増やしすぎず、

小さく・軽く・単機能・移植しやすく。

を優先しています。

License

ライセンスは公開前に決定予定です。

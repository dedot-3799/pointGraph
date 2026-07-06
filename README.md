# pointGraph

`pointGraph` は、点をドラッグしたり座標を直接入力したりしながら、2次関数や多項式のグラフを確認できる Blazor WebAssembly アプリです。

## できること

- 点を追加・削除して多項式を再計算
- 点をドラッグしてグラフを直感的に編集
- 座標入力による正確な調整
- 2次関数の一般形と頂点形式の変換確認
- 頂点と対称軸の表示

## 開発環境

- .NET 10 SDK
- Blazor WebAssembly

## 起動方法

```bash
dotnet restore
dotnet watch run
```

ブラウザで起動したら、ルートページでグラフ編集を試せます。

## 主な構成

- `Pages/` - 画面コンポーネント
- `Math/` - 多項式計算や座標変換
- `Models/` - 点データなどのモデル
- `wwwroot/` - 静的ファイル

## ライセンス

このプロジェクトは MIT License で公開しています。詳細は [LICENSE](LICENSE) を参照してください。

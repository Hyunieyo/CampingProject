Add-Type -AssemblyName System.Drawing

$assetDir = Join-Path (Get-Location) "assets"
New-Item -ItemType Directory -Force -Path $assetDir | Out-Null

function New-Scene {
  param(
    [string] $Path,
    [int] $Width,
    [int] $Height,
    [string] $Mode
  )

  $bitmap = New-Object System.Drawing.Bitmap $Width, $Height
  $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
  $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias

  $skyTop = [System.Drawing.Color]::FromArgb(218, 237, 230)
  $skyBottom = [System.Drawing.Color]::FromArgb(246, 209, 139)
  if ($Mode -eq "night") {
    $skyTop = [System.Drawing.Color]::FromArgb(35, 55, 65)
    $skyBottom = [System.Drawing.Color]::FromArgb(99, 112, 91)
  }
  if ($Mode -eq "lake") {
    $skyTop = [System.Drawing.Color]::FromArgb(204, 232, 239)
    $skyBottom = [System.Drawing.Color]::FromArgb(235, 242, 222)
  }

  $rect = New-Object System.Drawing.Rectangle 0, 0, $Width, $Height
  $brush = New-Object System.Drawing.Drawing2D.LinearGradientBrush $rect, $skyTop, $skyBottom, 90
  $graphics.FillRectangle($brush, $rect)

  $hillBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(44, 91, 69))
  $farHill = New-Object System.Drawing.Drawing2D.GraphicsPath
  $farHill.AddBezier(0, $Height * 0.54, $Width * 0.22, $Height * 0.31, $Width * 0.42, $Height * 0.53, $Width * 0.66, $Height * 0.38)
  $farHill.AddBezier($Width * 0.66, $Height * 0.38, $Width * 0.82, $Height * 0.3, $Width * 0.94, $Height * 0.52, $Width, $Height * 0.45)
  $farHill.AddLine($Width, $Height)
  $farHill.AddLine(0, $Height)
  $graphics.FillPath($hillBrush, $farHill)

  $groundBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(28, 65, 45))
  $graphics.FillEllipse($groundBrush, -80, $Height * 0.56, $Width + 160, $Height * 0.68)

  if ($Mode -eq "lake") {
    $waterBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(94, 150, 153))
    $graphics.FillEllipse($waterBrush, -50, $Height * 0.68, $Width + 100, $Height * 0.3)
  }

  $treeBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(22, 75, 54))
  $trunkBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(91, 63, 39))
  for ($i = 0; $i -lt 12; $i++) {
    $x = 30 + ($i * ($Width - 60) / 11)
    $scale = 0.72 + (($i % 4) * 0.12)
    $base = $Height * (0.68 + (($i % 3) * 0.03))
    $graphics.FillRectangle($trunkBrush, $x, $base - (44 * $scale), 8 * $scale, 46 * $scale)
    $graphics.FillPolygon($treeBrush, @(
      [System.Drawing.PointF]::new($x - 32 * $scale, $base - 35 * $scale),
      [System.Drawing.PointF]::new($x + 4 * $scale, $base - 118 * $scale),
      [System.Drawing.PointF]::new($x + 40 * $scale, $base - 35 * $scale)
    ))
    $graphics.FillPolygon($treeBrush, @(
      [System.Drawing.PointF]::new($x - 26 * $scale, $base - 75 * $scale),
      [System.Drawing.PointF]::new($x + 4 * $scale, $base - 146 * $scale),
      [System.Drawing.PointF]::new($x + 34 * $scale, $base - 75 * $scale)
    ))
  }

  $tentX = $Width * 0.42
  $tentY = $Height * 0.64
  $tentW = $Width * 0.26
  $tentH = $Height * 0.18
  $tentBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(213, 115, 72))
  $tentDark = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(130, 72, 48))
  $graphics.FillPolygon($tentBrush, @(
    [System.Drawing.PointF]::new($tentX, $tentY + $tentH),
    [System.Drawing.PointF]::new($tentX + $tentW * 0.5, $tentY),
    [System.Drawing.PointF]::new($tentX + $tentW, $tentY + $tentH)
  ))
  $graphics.FillPolygon($tentDark, @(
    [System.Drawing.PointF]::new($tentX + $tentW * 0.5, $tentY),
    [System.Drawing.PointF]::new($tentX + $tentW * 0.72, $tentY + $tentH),
    [System.Drawing.PointF]::new($tentX + $tentW * 0.32, $tentY + $tentH)
  ))

  $fireBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(245, 184, 77))
  $fireRed = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(203, 78, 52))
  $fireX = $Width * 0.34
  $fireY = $Height * 0.78
  $graphics.FillEllipse($fireRed, $fireX, $fireY - 22, 44, 44)
  $graphics.FillEllipse($fireBrush, $fireX + 9, $fireY - 34, 24, 42)

  $bitmap.Save($Path, [System.Drawing.Imaging.ImageFormat]::Png)
  $graphics.Dispose()
  $bitmap.Dispose()
}

New-Scene -Path (Join-Path $assetDir "camp-hero.png") -Width 1600 -Height 900 -Mode "night"
New-Scene -Path (Join-Path $assetDir "camp-feed.png") -Width 900 -Height 620 -Mode "day"
New-Scene -Path (Join-Path $assetDir "camp-gear.png") -Width 900 -Height 620 -Mode "night"
New-Scene -Path (Join-Path $assetDir "camp-lake.png") -Width 700 -Height 520 -Mode "lake"
New-Scene -Path (Join-Path $assetDir "camp-forest.png") -Width 700 -Height 520 -Mode "day"

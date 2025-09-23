# FlexPanel for WPF 🎯

[![.NET](https://img.shields.io/badge/.NET-5.0%2B-blue.svg)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/WPF-6.0%2B-purple.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen.svg)](#)
[![NuGet](https://img.shields.io/badge/NuGet-Coming%20Soon-orange.svg)](#)

>  CSS Flexbox-inspired layout panel for WPF applications. Bringing modern web layout capabilities to desktop development with enterprise-grade reliability.

## Quick Start

### Basic Usage

```xml
<Window x:Class="YourApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:Flex="clr-namespace:WpfFlexPanel.Core;assembly=WpfFlexPanel">
    <Grid>
        <Flex:FlexPanel FlexDirection="Row" 
                        JustifyContent="SpaceBetween" 
                        AlignItems="Center"
                        Gap="10">
            
            <Button Content="Button 1" flex:FlexPanel.FlexGrow="1" />
            <Button Content="Button 2" flex:FlexPanel.FlexGrow="2" />
            <Button Content="Button 3" flex:FlexPanel.FlexBasis="100" />
            
        </Flex:FlexPanel>
    </Grid>
</Window>
```

## 📖 Documentation

### Core Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `FlexDirection` | `FlexDirection` | `Row` | Main axis direction (`Row`, `Column`, `RowReverse`, `ColumnReverse`) |
| `JustifyContent` | `JustifyContent` | `FlexStart` | Main axis alignment (`FlexStart`, `FlexEnd`, `Center`, `SpaceBetween`, `SpaceAround`, `SpaceEvenly`) |
| `AlignItems` | `AlignItems` | `Stretch` | Cross axis alignment (`FlexStart`, `FlexEnd`, `Center`, `Stretch`) |
| `FlexWrap` | `FlexWrap` | `NoWrap` | Wrapping behavior (`NoWrap`, `Wrap`, `WrapReverse`) |
| `Gap` | `double` | `0` | Space between items |

### Attached Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `FlexGrow` | `double` | `0` | How much the item should grow |
| `FlexShrink` | `double` | `1` | How much the item should shrink |
| `FlexBasis` | `double` | `NaN` | Initial main size before free space distribution |
| `AlignSelf` | `AlignItems?` | `null` | Override parent's AlignItems for this item |
| `Order` | `int` | `0` | Display order (lower values first) |

## 🎯 Examples

### Card Layout with Wrapping

```xml
  <Flex:FlexPanel FlexDirection="Row" 
                  FlexWrap="Wrap" 
                  JustifyContent="SpaceBetween" 
                  Gap="15"
                  Background="WhiteSmoke">

      <Border Background="White" BorderBrush="LightGray" BorderThickness="1" CornerRadius="8" 
              Flex:FlexPanel.FlexBasis="200"  VerticalAlignment="Top"   Padding="15">
          <StackPanel>
              <TextBlock Text="Card 1" FontWeight="Bold" FontSize="16" Margin="0,0,0,10"/>
              <TextBlock Text="flexible card " TextWrapping="Wrap"/>
          </StackPanel>
      </Border>

      <Border Background="White" BorderBrush="LightGray" BorderThickness="1" CornerRadius="8" 
              Flex:FlexPanel.FlexBasis="200" Padding="15" VerticalAlignment="Top" >
          <StackPanel>
              <TextBlock Text="Card 2" FontWeight="Bold" FontSize="16" Margin="0,0,0,10"/>
              <TextBlock Text="Another flexible card " TextWrapping="Wrap"/>
          </StackPanel>
      </Border>

      <Border Background="White" BorderBrush="LightGray" BorderThickness="1" CornerRadius="8" 
              Flex:FlexPanel.FlexBasis="200" Padding="15" VerticalAlignment="Top" >
          <StackPanel>
              <TextBlock Text="Card 3" FontWeight="Bold" FontSize="16" Margin="0,0,0,10"/>
              <TextBlock Text="Another flexible card" TextWrapping="Wrap"/>
          </StackPanel>
      </Border>
  </Flex:FlexPanel>
```



## 📊 CSS Support Comparison

| Feature | FlexPanel | CSS Flexbox |
|---------|-----------|-------------|
| flex-direction | ✅ | ✅ |
| flex-wrap | ✅ | ✅ |
| justify-content | ✅ | ✅ |
| align-items | ✅ | ✅ |
| align-self | ✅ | ✅ |
| flex-grow | ✅ | ✅ |
| flex-shrink | ✅ | ✅ |
| flex-basis | ✅ | ✅ |
| gap | ✅ | ✅ |
| order | ✅ | ✅ |

# Nasa Asteroid Visual

## Obsah

1. [Úvod](#1-úvod)
   1.1 [Účel](#11-účel)  
   1.2 [Konvence dokumentu](#12-konvence-dokumentu)  
   1.3 [Cílová skupina](#13-cílová-skupina)  
   1.4 [Kontakty](#14-kontakty)  
2. [Popis](#2-popis)
   2.1 [Produkt](#21-produkt)  
   2.2 [Funkce](#22-funkce)  
   2.3 [Uživatelské skupiny](#23-uživatelské-skupiny)  
   2.4 [Provozní prostředí](#24-provozní-prostředí)  
   2.5 [Uživatelské prostředí](#25-uživatelské-prostředí)  
   2.6 [Předpoklady a závislosti](#26-předpoklady-a-závislosti)  
3. [Požadavky na rozhraní](#3-požadavky-na-rozhraní)
   3.1 [Uživatelské rozhraní](#31-uživatelské-rozhraní)  
4. [Vlastnosti systému](#4-vlastnosti-systému)
   4.1 [Zobrazení asteroidů](#41-zobrazení-asteroidů)  
   4.2 [Filtrování asteroidů](#42-filtrování-asteroidů)  
5. [Nefunkční požadavky](#5-nefunkční-požadavky)
   5.1 [Výkonnost](#51-výkonnost)  
   5.2 [Bezpečnost](#52-bezpečnost)  
   5.3 [Spolehlivost](#53-spolehlivost)  
   5.4 [Projektová dokumentace](#54-projektová-dokumentace)  

## Historie dokumentu

| Verze | Autor          | Komentář                          |
|-------|----------------|-----------------------------------|
| 1     | Vasko Daniel    | První verze dokumentu            |

## 1. Úvod

### 1.1 Účel

Zobrazit vesmírné tělesa ve 2D zobrazení z daného datumu.

### 1.2 Konvence dokumentu

Exe soubor v jazyku C#.

### 1.3 Cílová skupina

Tento dokument je určený pro ty, kdo se chtějí vidět vesmírná tělesa zachycená a poskytnutá od NASA API.

### 1.4 Kontakty

Mail: <vaskodaniel1@gmail.com>

## 2. Popis

### 2.1 Produkt

Produkt bude vyvíjen v jazyku C# a bude spouštěn jako .exe soubor.

### 2.2 Funkce

Funkce budou, **přidávání a ukládání** vlastního NASA API klíče pro přístup k API, po vybrání datumu **zobrazení asteroidů** zaznamenané v API.

### 2.3 Uživatelské skupiny

Bude pouze jedna uživatelská skupina a to uživatelé, kteří mají přístup ke všem funkcím aplikace.

### 2.4 Provozní prostředí

Tato aplikace je cílená pouze na Android zařízení.

### 2.5 Uživatelské prostředí

Input pro NASA API klíč. Datový input pro zadání dne, který je vyžadován zobrazit a tlačítko, které potvrdí výběr a zapne zobrazení asteroidů.

### 2.6 Předpoklady a závislosti

Předpoklad, že si uživatel vytvoří účet na <https://api.nasa.gov> a použije jeho vlastní API klíč. Malé využití CPU a paměti, vyžadováno připojení k wifi, jednoduché a rychlé používání.

## 3. Požadavky na rozhraní

### 3.1 Uživatelské rozhraní

Mobilní aplikace, kde na jedné stránce bude moct filtrovat a ihned zobrazit asteroidy.

## 4. Vlastnosti systému

### 4.1 Zobrazení asteroidů

- Důležitost: **HIGH**
- Zobrazení asteroidů s **intensitou barvy** podle jejich "absolute_magnitude_h" vlastnosti. **Červené ohraničení** pokud je asteroid potenciálně hazardní podle vlastnosti "is_potentially_hazardous_asteroid".

### 4.2 Filtrování asteroidů

- Důležitost: **HIGH**
- Filtrování asteroidů podle jejich vlastností jako je velikost, nebezpečnost.

## 5. Nefunkční požadavky

### 5.1 Výkonnost

- Výkonnost této aplikace záleží na rychlosti odpovědi API, tudíž nelze specifikovat požadovanou rychlost aplikace.

### 5.2 Bezpečnost

- Vzhledem k tomu, že tato aplikace neobsahuje žádné citlivé informace až na NASA API klíč, není potřeba řešit.

### 5.3 Spolehlivost

- Funkčnost aplikace (zapnutí, vypnutí).
- Zajistit správné zobrazení asteroidů.

### 5.4 Projektová dokumentace

- Současný dokument.

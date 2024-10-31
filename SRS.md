# Nasa Asteroid Visual - Softwarové požadavky

- Verze 2
- Autor: Daniel Vaško
- Datum: 31.10.

<div style="page-break-after: always;"></div>

## Obsah
|
1. [Úvod](#1-Úvod)

   1.1. [Účel](#11-Účel)
   1.2. [Cílová skupina](#13-Cílová-skupina)
   1.3. [Kontakty](#14-Kontakty)

2. [Popis](#2-Popis)

   2.1. [Produkt](#21-Produkt)
   2.2. [Funkce](#22-Funkce)
   2.3. [Uživatelské skupiny](#23-Uživatelské-skupiny)
   2.4. [Provozní prostředí](#24-Provozní-prostředí)
   2.5. [Uživatelské prostředí](#25-Uživatelské-prostředí)
   2.6. [Předpoklady a závislosti](#26-Předpoklady-a-závislosti)

3. [Požadavky na rozhraní](#3-Požadavky-na-rozhraní)

   3.1. [Uživatelské rozhraní](#31-Uživatelské-rozhraní)

4. [Funkční požadavky](#4-Funkční-požadavky)

   4.1. [Zobrazení asteroidů](#41-Zobrazení-asteroidů)
   4.2. [Filtrování asteroidů](#42-Filtrování-asteroidů)
   4.3. [Zadání API tokenu](#43-Zadání-API-tokenu)
   4.4. [Offline režim](#44-Offline-režim)

5. [Nefunkční požadavky](#5-Nefunkční-požadavky)

   5.1. [Výkonnost](#51-Výkonnost)
   5.2. [Bezpečnost](#52-Bezpečnost)
   5.3. [Spolehlivost](#53-Spolehlivost)
   5.4. [Projektová dokumentace](#54-Projektová-dokumentace)

<div style="page-break-after: always;"></div>

## Historie dokumentu

| Verze | Datum | Autor          | Komentář                          |
|-------|-------|--------|-----------------------------------|
| 1     | 22.10. |Vasko Daniel    | První verze dokumentu            |
| 2     | 31.10. |Vasko Daniel    | Přidání první stránky, 4.3, 4.4 a oprava 5.1|

## 1. Úvod

### 1.1 Účel

Zobrazit vesmírné tělesa ve 2D zobrazení z daného datumu.

### 1.3 Cílová skupina

Tento dokument je určený pro ty, kdo se chtějí vidět vesmírná tělesa zachycená a poskytnutá od NASA API.

### 1.4 Kontakty

Mail: <vaskodaniel1@gmail.com>

## 2. Popis

### 2.1 Produkt

Produkt bude vyvíjen v jazyku C# a bude spouštěn jako mobilní aplikace.

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

## 4. Funkční požadavky

### 4.1 Zobrazení asteroidů

- Popis: Zobrazení asteroidů s **intensitou barvy** podle jejich "absolute_magnitude_h" vlastnosti. **Červené ohraničení** pokud je asteroid potenciálně hazardní podle vlastnosti "is_potentially_hazardous_asteroid".
- Důležitost: **HIGH**

### 4.2 Filtrování asteroidů

- Popis: Filtrování asteroidů podle jejich vlastností jako je velikost, nebezpečnost.
- Důležitost: **HIGH**

### 4.3 Zadání API tokenu

- Popis: Přidání API tokenu pro získání dat z <https://api.nasa.gov>.
- Důležitost: **HIGH**

### 4.4. Offline režim

- Popis: V případě, že se uživatel pokusí udělat něco co vyžaduje přístup k internetu a aplikace nemá přístup k internetu dostane uživatel upozornění, že aplikace bez internetu nemůže fungovat. Toto upozornění bude možné zavřít.
- Důležitost: **MEDIUM**

<div style="page-break-after: always;"></div>

## 5. Nefunkční požadavky

### 5.1 Výkonnost

- Výkonnost této aplikace záleží na rychlosti odpovědi API, tudíž specifikujeme požadovanou rychlost aplikace bez počítání odpovědi API a to tedy až 10ms za každý vykreslovaný objekt se základem 1s.

### 5.2 Bezpečnost

- Vzhledem k tomu, že tato aplikace neobsahuje žádné citlivé informace až na NASA API klíč, není potřeba řešit.

### 5.3 Spolehlivost

- Funkčnost aplikace (zapnutí, vypnutí).
- Zajistit správné zobrazení asteroidů.

### 5.4 Projektová dokumentace

- Současný dokument.

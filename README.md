# M318 SwissTransport
## 1 Inhaltsverzeichnis
* [2 Projekt Informationen](#information) 
* [3 Konzeptionierung](#concept)  
  * [3.1 Coderichtlinie](#code) 
  * [3.2 Mockup](#mockup)  
  * [3.3 Validierung](#validation)  
  * [3.4 Testfälle](#test) 
  * [3.5 Use Case](#usecase)  
  * [3.6 Aktivitätendiagramm](#activity) 
* [4 Umsetzung](#implementation)  
  * [4.1 Funktionen](#features)  
  * [4.2 fehlede Funktionen und Bugs](#bugs)  
  * [4.3 Screenshots](#screenshot) 
* [5 Testbericht](#report)
* [6 Installation](#install)  

<a name="information"/>

# 2 Projekt Informationen
## 2.1 Autor und Dokument
* Autor: **Gian Ott**
* Erstelldatum: **22.04.2018**
* letzte Aktualisierung: **24.04.2018**

## 2.2 Projekt
* Projektstart: **18.04.2018**   
* Projektende: **24.04.2018** 
* Der Source sowie die Dokumentation sind auf GitHub unter **https://github.com/Nichtgian/SwissTransport**
* Download der Installationsdatei SwissTransportSetup.msi **https://github.com/Nichtgian/SwissTransport/blob/master/SwissTransportSetup.msi?raw=true**

## 2.3 Einleitung
Im ÜK Modul 318 wurde zur Festigung des Gelernten und zur Bewertung eine Projektarbeit, inklusive Dokumentation in einer Einzelarbeit erstellt.
Das Projekt soll als Desktop Anwendung auf eine API zugreifen und anschliessend diverse Daten wie Verbindungen zwischen Stationen anzeigen.
Die Applikation ist zusammen mit der Dokumentation auf GitHub zu finden.

### 2.3.1 Management Summary
Dies SwissTransport Desktop-Applikation soll es ihren Benutzern erlauben, zum einen Reisen und Ausflüge am aktuellen Datum und Zeitpunkt oder an einem zukünftigen Datum und einem anderen Zeitpunkt zu Planen.
Sie erhalten durch die Angaben eine Liste mit den nächsten Verbindungen, und können dann genaue Details wie das exakte Datum, die Reisedauer oder die Gleisnummern abrufen.
Zum anderen kann nach allen Abfahrten ab einer Station gesucht werden. 
Man erhält eine Liste mit genauen Details zu den einzelnen Abfahrten beinhaltend des Reisezieles und der Abfahrtsidentifikation wie IR70.
Die Applikation meldet dem Benutzer falsche Eingaben direkt zurück und übernimmt das Vervollständigen von nicht fertig eingegebenen Stationsnamen. 

## 2.4 Dokumentation
Diese Dokumentation beinhaltet die Planung, die Umsetzung, und das Endergebnis zusammen mit Testfällen und einer Installationsanleitung.
Sie soll als Nachschlagewerk dienen und wurde für eine einfache Handhabung und Zugänglichkeit im .md (Readme) Format geschrieben. 

## 2.5 Anforderungen
### 2.5.1 Priorität 1
- [x] 01 Start- und Endstationen mittels Eingabefeld suchen
- [x] 02 Die nächste vier bis fünf Verbindungen zwischen angegebener Start- und Endstationen sehen
- [x] 03 Verbindungen ab einer bestimmten Station anzeigen

### 2.5.2 Priorität 2
- [x] 04 Während der Eingabe einer Station bereits Suchresultate erhalten
- [x] 05 Verbindungen an beliebigem Datum anzeigen

### 2.5.3 Priorität 3
- [x] 06 Ort und Umgebung einer Station anzeigen (Maps)
- [ ] 07 Stationen in meiner Nähe finden
- [ ] 08 Gefundene Resultate per Mail verschicken

<a name="concept"/>

# 3 Konzeptionierung
Mit C# und WPF werde ich nach dem MVVM Modell arbeiten. Die komplette Logik werde ich in Models auslagern, 
wodurch keine Logik im Code behind von Views und Pages steht und diese nur Events von Controls entgegennehmen. 

<a name="code"/>

## 3.1 Coderichtlinien
### 3.1.1 Namensgebung
* **Variablen und Methoden** werden nach **camelCase** benannt -> erster Buchstabe klein, Wortanfänge im Namen gross
* **Eigenschaften** werden nach **PascalCase** benannt -> erster Buchstabe gross, Wortanfänge im Namen gross
```csharp
public string camelCase = "";
public string PascalCase { get; set; }
```


### 3.1.2 Methoden, Schleifen, Verzweigungen, Try Catch
In C# kommen geschweifte Klammern grundsätzlich auf die **nächste Zeile**, weshalb ich dies bei Methoden, Schleifen und Verzweigungen einhalten werden.
```csharp
if (true)
{
    true = false;
}
else
{
    false = true;
}
```
Ich **verzichte auf die Kurzschreibweise** ohne geschweifte Klammern bei if-Verzweigungen, werde aber bei bool Parameter
den **ternären Operator** ( ? : ) anstatt if else brauchen.
```csharp
getSomething(true ? true : false);
```

### 3.1.3 Kommentare
Ich kommentiere grundsätzlich **jede Methode oberhalb** mit /* */, verzichte ansonsten auf Kommentare.
```csharp
/*returns station from input*/
public void getStation(string input)
{
    return api.getStation(input);
}
```

### 3.1.4 Eigenschafte
Die Namensgebung von Eigenschaften (Propertys) ist in PascalCase. Geschweifte Klammern werde ich bei kurzer Schreibweise auf einer Zeile und bei ausgeschriebenen Eigenschaften auf die nächste Zeile schreiben.
```csharp
private string country;
public string Country
{
    get { return country }
    set { country = value; anyFunction(); }
}
public string City { get; set; }
```

<a name="mockup"/>

## 3.2 Mockup
Für die beiden Hauptfunktionalitäten Verbindungen suchen und Abfahrtsplan anzeigen werde ich zwei Seiten erstellen. 
Diese beiden Seiten werden in einer Hauptseite (SwissTransportView) geladen, welche nur eine Menüleiste zur Navigation zwischen den Seiten beinhaltet.
### 3.2.1 Verbindungen
**StationPage** Es können zwei Stationen oder Orte (Von und Nach) angegeben werden. 
Standartmässig wird das aktuelle Datum und Zeit eingefüllt, jedoch kann das Abfahrtsdatum und die Abfahrtszeit beliebig angegeben werden.
Wird nach Verbindungen gesucht, werden von der API die nächsten Verbindungen anhand der Eingaben zurückgegeben, welche in die Liste gefüllt werden. 
Es werden pro Verbindung die Vonstation und Abfahrtszeit sowie der Nachstation und Ankunftszeit angezeigt. 
Wird ein Listenelement angewählt, erscheint ein neues Fenster mit mehr Details (Vonstation, genaue Abfahrtszeit sowie dem Gleis, 
die Reisezeit und die Nachstation mit genauer Ankunftszeit und Gleis). Bei Falschen Angaben erscheint eine Nachricht, welche die fehlerhaften
Angaben zurückmeldet.

In der Menüleiste kann auf die Abfahrtsplans Seite gewechselt werden.

![Mockup Verbindungen](/img/StationMockup.JPG)

### 3.2.2 Abfahrtsplan
**BoardPage** Es kann eine Station angegeben werden. Mit Abfahrtsplan anzeigen werden alle Abfahrten von der Station ausgehend in die Liste gefüllt.
Es werden der Name, seine Zugskategorie, die Gleisnummer und die Ankunftsstation angezeigt. 
Mit Stationsort anzeigen wird Google Maps in einem Browserfenster an der angegebenen Station geöffnet.

In der Menüleiste kann auf die Verbindungen Seite gewechselt werden.  

![Mockup Abfahrtsplan](/img/BoardMockup.JPG)
  
<a name="validation"/>

## 3.3 Validierung
* **Stationen** Es werden direkt bei der Eingabe Suchresultate in einem Dropdown angezeigt. Diese Resultate werden nach Wichtigkeit und Zusammenpassen sortiert angezeigt 
und können angeklickt werden, was zur Vervollständigung der Eingabe führt. Wurde ein Stationsname bei einer Verbindungs- oder Abfahrtsplanssuche fertig geschrieben, 
wird nach einem Match gesucht und eingefügt. Falls keiner gefunden wurde wird dies gemeldet.
Stationsnamen dürfen nicht leer und bei der Verbindungsseite die Abfahrts- und Ankunftsstation nicht gleich sein.

* **Datum** Das Datum wird nach dem aktuellen Datum standartmässig ausgefüllt. Wird nach einer Verbindung gesucht, darf das Datum nicht leer sein
und es muss ein gültiges Datum sein, ansonsten wird dies in einem Fenster mitgeteilt.

* **Zeit** Die Zeit wird nach der aktuellen Zeit standartmässig ausgefüllt. Wird nach einer Verbindung gesucht, darf die Zeit nicht leer sein
und es muss eine gültige Zeit sein, ansonsten wird dies in einem Fenster mitgeteilt.

<a name="test"/>

## 3.4 Testfälle
### 3.4.1 Station suchen
**Vorbedingung** Es kann nach einer Station gesucht werden (Verbindungs- oder Abfahrtsplansseite). 

**Anforderung** A01 & A04

**Testszenario**

| Schritt | Aktivität                                                     | Erwartetes Resultat                                      |
| ------- | ------------------------------------------------------------- | -------------------------------------------------------- | 
| 1       | Ich gebe "zu" in das Eingabefeld Von auf der Verbindungsseite ein | Es wird eine Dropdown Liste unter dem Eingabefeld mit den Elementen Zürich HB, Zürich Oerlikon, Zürich Flughafen.. angezeigt |
| 2       | Ich gebe "zu" in das Eingabefeld Nach auf der Verbindungsseite ein | Es wird eine Dropdown Liste unter dem Eingabefeld mit den Elementen Zürich HB, Zürich Oerlikon, Zürich Flughafen.. angezeigt |
| 3       | Ich gebe "zu" in das Eingabefeld Station auf der Abfahrtsplansseite ein | Es wird eine Dropdown Liste unter dem Eingabefeld mit den Elementen Zürich HB, Zürich Oerlikon, Zürich Flughafen.. angezeigt |
| 4       | Ich wähle "Zürich HB" aus der Dropdown Liste unter Von auf der Verbindungsseite aus | Zürich HB steht im Eingabefeld Von |

### 3.4.2 Station suchen
**Vorbedingung** Ich bin auf der Verbindungsseite. 

**Anforderung** A02

**Testszenario**

| Schritt | Aktivität                                                     | Erwartetes Resultat                                      |
| ------- | ------------------------------------------------------------- | -------------------------------------------------------- | 
| 1       | Ich lasse das Eingabefeld Von frei                            | Wenn nach einer Verbindung gesucht wird, erscheint in einer Mitteilung, dass die Von Station nicht ausgefüllt ist |
| 2       | Ich lasse das Eingabefeld Nach frei                           | Wenn nach einer Verbindung gesucht wird, erscheint in einer Mitteilung, dass die Nach Station nicht ausgefüllt ist |
| 3       | Ich gebe in das Eingabefeld Von "lu" ein                      | Wenn nach einer Verbindung gesucht wird, wird in das Eingabefeld Von "Luzern" eingefüllt |
| 4       | Ich gebe in das Eingabefeld Nach "zu" ein                     | Wenn nach einer Verbindung gesucht wird, wird in das Eingabefeld Von "Zürich HB" eingefüllt |
| 5       | Ich gebe in den Eingabefeldern Von "lu" und Nach "lu"         | Wenn nach einer Verbindung gesucht wird, erscheint in einer Mitteilung, dass die Von "Luzern" und Nach "Luzern" Station nicht dieselben sein dürfen |
| 6       | Ich gebe in den Eingabefeldern Von "luz" und Nach "lu"        | Wenn nach einer Verbindung gesucht wird, erscheint in einer Mitteilung, dass die Von "Luzern "und Nach "Luzern" Station nicht dieselben sein dürfen |
| 7       | Ich gebe in das Eingabefeld Von "Mettmenstetten" und Nach "Zug" ein und suche nach einer Verbindung | Es erscheint eine Liste mit den nächsten Verbindungen von Mettmenstetten nach Zug |
| 8       | Ich gebe in das Eingabefeld Datum "www" ein                   | Das Eingabefeld wird beim Versetzen des Fokus auf den vorherigen Wert zurückgesetzt |
| 9       | Ich gebe in das Eingabefeld Zeit "www" ein                    | Wenn nach einer Verbindung gesucht wird, erscheint in einer Mitteilung dass die angegebene Zeit "www" ungültig ist |
| 10      | Ich gebe in das Eingabefeld Von "Mettmenstetten" und Nach "Zug" ein und suche nach einer Verbindung | Wenn ich auf eine Verbindung in der Liste drücke erscheint ein Fenster mit genaueren Details zur ausgewählten Verbindung |

### 3.4.3 Abfahrtsplan anzeigen
**Vorbedingung** Ich bin auf der Abfahrtsplansseite. 

**Anforderung** A03

**Testszenario**

| Schritt | Aktivität                                                     | Erwartetes Resultat                                      |
| ------- | ------------------------------------------------------------- | -------------------------------------------------------- | 
| 1       | Ich lasse das Eingabefeld Station frei                        | Wenn auf Abfahrtsplan anzeigen geklickt wird, erscheint in einer Mitteilung, dass die Station nicht ausgefüllt ist |
| 2       | Ich gebe in das Eingabefeld Station "lu" ein                  | Wenn nach einer Verbindung gesucht wird, wird in das Eingabefeld Station "Luzern" eingefüllt |
| 3       | Ich gebe in das Eingabefeld Station "Luzern" ein und klicke auf Abfahrtsplan anzeigen | Es wird eine Liste mit den nächsten Abfahrten ab Luzern angezeigt |
| 4       | Ich gebe in das Eingabefeld Station "Luzern" ein und klicke auf Stationsort anzeigen | Es wird Google Maps in einem Browserfenster geöffnet und darin wird Luzern angezeigt |

<a name="usecase"/>

## 3.5 Use Case
Unsere Anwendung wird von Reisenden benutzt. Diese können nach Stationen suchen, aktuelle oder zukünftige Verbindungen zwischen
zwei Stationen anzeigen und einen Abfahrtsplan von einer Station aus ausgehend anzeigen lassen.

![Use Case](/img/UseCase.JPG)

<a name="activity"/>

## 3.6 Aktivitätendiagramm
Der grobe Ablauf (alle Priorität 1 Anforderungen) der Anwendung ist in diesem Aktivitätendiagramm zusammengefasst.

![Aktivitätendiagramm](/img/activity.JPG)

<a name="implementation"/>

# 4 Umsetzung

<a name="features"/>

## 4.1 Funktionen
| Anforderung | Priorität | Status | Beschreibung                      |
| ----------- | --------- | ------ | --------------------------------- | 
| A01         | 1         | ✔      | Während der Eingabe einer Station werden Vorschläge in einem Dropdown angezeigt | 
| A02         | 1         | ✔      | Verbindungsseite: Mit Verbindung suchen werden die nächsten Verbindungen anhand der Angaben gesucht und angezeigt | 
| A03         | 1         | ✔      | Abfahrtsplansseite: Mit Abfahrtsplan anzeigen werden die nächste Abfahrten ab der eingegebenen Station gesucht und angezeigt | 
| A04         | 2         | ✔      | Während der Eingabe einer Station werden Vorschläge in einem Dropdown angezeigt | 
| A05         | 2         | ✔      | Es kann ein beliebiges Datum und oder Zeit zur Verbindungssuche angegeben werden, standartmässig aktuelle Zeit |
| A06         | 3         | ✔      | Mit Stationsort anzeigen wird Maps in einem Browserfenster an der angegebener Station geöffnet |  

✔ **Funktion fehlerfrei implementiert**

### 4.1.1 zusätzliche Funktionen
**Autocomplete Station** Ist bei einer Station nur der Anfang (zB. ba) eingegeben worden, 
wird bei einer Suche (Verbindung suchen oder Abfahrtsplan anzeigen) nach einem besten Vorschlag gesucht 
und in das Eingabefeld eingefügt (hier Basel SBB). Wird keine Station gefunden wird dies Mitgeteilt.

<a name="bugs"/>

## 4.2 fehlende Funktionen und Bugs
### 4.2.1 Fehlende, teilweise oder nicht fertige Funktionen
| Anforderung | Priorität | Status | Beschreibung                      |
| ----------- | --------- | ------ | --------------------------------- | 
| A07         | 3         | 🔶      | Es können keine Stationen in der Nähe des Standorts gefunden werden | 
| A08         | 3         | 🔶      | Resultate können nicht per Email verschickt werden | 

🔶 **Funktion nicht implementiert**

🐛 **Funktion nur teilweise implementiert und fertig**

❌ **Funktion führt zu Fehler und ist nicht fertig und funktionstüchtig**

### 4.2.2 Bugs und Fehler
Zurzeit sind alle **Bugs behoben** und **keine** weiteren Fehler bekannt.

<a name="screenshot"/>

## 4.3 Screenshots der Anwendung
### 4.3.1 Verbindungsseite

![Station](/img/connection.JPG)

### 4.3.2 Abfahrtsplansseite

![Board](/img/board.JPG)

<a name="report"/>

# 5 Testbericht
* Die Unit Tests wurden fehlerfrei durchgeführt.
* Alle Testfälle konnten ohne Fehler durchgeführt werden.
* Alle implementierten Funktionalitäten können ohne Probleme ausgeführt werden.

<a name="install"/>

# 6 Installation
## 6.1 Download
Die Setupdatei kann direkt von GitHub heruntergeladen werden und wird im Download Ordner gespeichert.

[<img src="/img/download.jpg">](https://github.com/Nichtgian/SwissTransport/blob/master/SwissTransportSetup.msi?raw=true)

oder

**https://github.com/Nichtgian/SwissTransport/blob/master/SwissTransportSetup.msi?raw=true** 

## 6.2 Setup
Gehe in den Downloads Ordner oder führe die **Setupdatei SwissTransportSetup.msi** direkt vom Browser aus aus. 
Falls eine Meldung eingeblendet wird, dass der Computer durch Windows geschütz wurde, klicke auf **Weitere Informationen** und **Trotzdem ausführen**, um mit der Installation zu beginnen.

Klicke auf **Next**

Die Anwendung wird standartmässig im Program Files Ordner installiert, wenn sie aber in einen anderen Ordner installiert werden soll klicke auf **Browse** 
und wähle einen anderen Ordner. Du kannst ausserdem auswählen, ob die Anwendung für alle Benutzer des Computers (Everyone) oder nur für dich **Just me** zugänglich sein soll.
Mit **Next** kannst du deine Eingaben bestätigen.

Klicke auf **Next** um die Anwendung zu installieren.

Die Anwendung ist nun auf deinem Computer im festgelegten Ordner installiert. Die Setupdatei kann ausserdem zur **Reperatur und Deinstallation** der Anwendung genutzt werden.

> Best SwissTransport Application in the whole Universe!


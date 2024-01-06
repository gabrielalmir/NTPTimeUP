package main

import (
	"bufio"
	"flag"
	"os"

	ntptime "github.com/gabrielalmir/NTPTimeUP/ntp"
)

var ntpServer = flag.String("host", "a.st1.ntp.br", "Define the ntp server host (default: a.st1.ntp.br)")
var ntpPort = flag.String("port", "123", "Define the ntp port host (default: 123)")
var skipFlag = flag.Bool("skip", false, "Skip applying time (default: false)")

func main() {
	flag.Parse()

	ntptime.SyncLocalTime(*ntpServer, *ntpPort, *skipFlag)

	println("Press <ENTER> to close the application ...")
	reader := bufio.NewReader(os.Stdin)
	reader.ReadLine()
}

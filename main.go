package main

import (
	"bufio"
	"flag"
	"os"

	ntptime "github.com/gabrielalmir/NTPTimeUP/ntp"
)

var skipFlag = flag.Bool("skip", false, "Skip applying time (default: false)")

func main() {
	flag.Parse()

	ntptime.SyncLocalTime(*skipFlag)

	println("Press <ENTER> to close the application ...")
	reader := bufio.NewReader(os.Stdin)
	reader.ReadLine()
}

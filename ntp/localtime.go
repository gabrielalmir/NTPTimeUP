package ntptime

import (
	"encoding/binary"
	"fmt"
	"log"
	"net"
	"os/exec"
	"strings"
	"syscall"
	"time"
)

func getNetworkTime(ntpServer string, ntpPort string) (time.Time, error) {
	conn, err := net.Dial("udp", net.JoinHostPort(ntpServer, ntpPort))
	if err != nil {
		return time.Time{}, err
	}
	defer conn.Close()

	// Set timeout for receiving response
	conn.SetDeadline(time.Now().Add(3 * time.Second))

	// NTP message format (RFC 5905)
	ntpData := make([]byte, 48)
	ntpData[0] = 0x1b // LI = 0, VN = 3, Mode = 3

	if _, err := conn.Write(ntpData); err != nil {
		return time.Time{}, err
	}

	if _, err := conn.Read(ntpData); err != nil {
		return time.Time{}, err
	}

	// Extract Transmit Timestamp (seconds and fraction)
	transmitTimestamp := ntpData[40:48]
	intPart := binary.BigEndian.Uint32(transmitTimestamp[0:4])
	fractPart := binary.BigEndian.Uint32(transmitTimestamp[4:8])

	// Convert NTP seconds to Unix time
	unixSeconds := int64(intPart - 2208988800) // NTP epoch to Unix epoch offset
	unixNanoseconds := int64(uint64(fractPart) * 1e9 / 0x100000000)

	// Construct UTC time and adjust to local time zone
	utcTime := time.Unix(unixSeconds, unixNanoseconds).UTC()
	localTime := utcTime.In(time.Local)

	return localTime, nil
}

func setTimeCMD(clock string) bool {
	// Validate the input clock value
	_, err := time.Parse("15:04", clock)
	if err != nil {
		fmt.Println("Invalid clock value")
		return false
	}

	// Execute the time command with elevated privileges
	cmd := exec.Command("cmd", "/C", "time", clock)
	cmd.SysProcAttr = &syscall.SysProcAttr{HideWindow: false}

	// Start the command and capture any output
	output, err := cmd.CombinedOutput()
	if err != nil {
		fmt.Println("Error setting time:", err)
		fmt.Println(strings.TrimSpace(string(output))) // Print any error messages from the command
		return false
	}

	return true
}

func SyncLocalTime(ntpServer string, ntpPort string, skipApply bool) {
	systemTime := time.Now()
	localTime, err := getNetworkTime(ntpServer, ntpPort)

	if err != nil {
		log.Fatalln("Cannot get the correct local time")

	}

	// Format in Hour and Minute only
	timeInClock := fmt.Sprintf("%02d:%02d", localTime.Hour(), localTime.Minute())
	systemTimeInClock := fmt.Sprintf("%02d:%02d", systemTime.Hour(), systemTime.Minute())

	println("# NTP:", ntpServer, "Port:", ntpPort)
	println("# Current Time:", systemTimeInClock)
	println("# Correct Time:", timeInClock)

	if skipApply {
		println("* Skipping applying ...")
	} else {
		println("* Applying NTP Time ...")
		setTimeCMD(timeInClock)
	}
}

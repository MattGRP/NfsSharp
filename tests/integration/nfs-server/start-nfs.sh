#!/usr/bin/env bash
set -euo pipefail

mkdir -p /run/dbus
dbus-daemon --system --nofork &
dbus_pid=$!

rpcbind -w -f &
rpcbind_pid=$!

ganesha.nfsd -F -L STDOUT -f /etc/ganesha/ganesha.conf &
ganesha_pid=$!

shutdown()
{
    kill "$ganesha_pid" "$rpcbind_pid" "$dbus_pid" 2>/dev/null || true
    wait "$ganesha_pid" "$rpcbind_pid" "$dbus_pid" 2>/dev/null || true
}

trap shutdown EXIT INT TERM

set +e
wait -n "$rpcbind_pid" "$ganesha_pid" "$dbus_pid"
status=$?
set -e

exit "$status"

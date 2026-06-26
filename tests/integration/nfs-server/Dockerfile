FROM ubuntu:24.04@sha256:786a8b558f7be160c6c8c4a54f9a57274f3b4fb1491cf65146521ae77ff1dc54

ARG DEBIAN_FRONTEND=noninteractive

RUN apt-get update \
    && apt-get install --yes --no-install-recommends \
        nfs-ganesha=4.3-8ubuntu1 \
        nfs-ganesha-mem=4.3-8ubuntu1 \
        rpcbind=1.2.6-7ubuntu2 \
        tini=0.19.0-1 \
    && rm -rf /var/lib/apt/lists/*

COPY ganesha.conf /etc/ganesha/ganesha.conf
COPY start-nfs.sh /usr/local/bin/start-nfs.sh

RUN chmod 0755 /usr/local/bin/start-nfs.sh \
    && mkdir -p /run/ganesha /run/rpcbind

EXPOSE 111/tcp 2049/tcp 20048/tcp

ENTRYPOINT ["/usr/bin/tini", "--", "/usr/local/bin/start-nfs.sh"]

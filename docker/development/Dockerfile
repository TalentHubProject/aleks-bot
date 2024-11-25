FROM rust:bullseye

WORKDIR /app
COPY . .

RUN cargo install cargo-watch
RUN cargo install --path .

CMD ["cargo", "watch", "-x", "run"]
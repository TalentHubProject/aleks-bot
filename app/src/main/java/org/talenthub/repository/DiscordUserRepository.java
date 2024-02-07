package org.talenthub.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import org.talenthub.persistence.DiscordUser;

@Repository
public interface DiscordUserRepository extends JpaRepository<DiscordUser, Long> {
}

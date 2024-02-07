package org.talenthub.module.xp.infrastructure.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.talenthub.module.xp.infrastructure.persistence.Level;

import java.util.List;
import java.util.Optional;

@Repository
public interface LevelRepository extends JpaRepository<Level, Integer> {

    @Query("SELECT lvl FROM Level lvl WHERE lvl.maxXp > :xp ORDER BY lvl.id ASC LIMIT 1")
    Optional<Level> findNextLevelByMaxXp(@Param("xp") final long xp);

}

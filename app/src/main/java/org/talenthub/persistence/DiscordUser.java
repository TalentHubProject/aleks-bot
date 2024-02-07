package org.talenthub.persistence;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.talenthub.module.xp.infrastructure.persistence.Level;

@Entity
@Table
@Getter
@Setter
@NoArgsConstructor
public class DiscordUser {

    @Id
    private long id;

    @OneToOne
    private Level level;

    @Column
    private long xp;

    public DiscordUser(final long id, final Level level){
        this.id = id;
        this.level = level;
        this.xp = 0;
    }

}
